using Howestprime.Backoffice.Services;
using Howestprime.Backoffice.ViewModels.Planning;
using ApiMovie = Howestprime.Movies.ApiClient.Responses.Movie;
using ApiRoom = Howestprime.Movies.ApiClient.Responses.Room;
using ScheduleMovieEventRequest = Howestprime.Movies.ApiClient.Requests.ScheduleMovieEventRequest;
using Microsoft.AspNetCore.Components;
using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.Components.Pages;

public partial class Planning
{
    private static readonly string[] TimeItems = ["15:00", "19:00"];
    private static readonly string[] RoomLegendClasses = ["room-legend-blue", "room-legend-yellow", "room-legend-green"];
    private static readonly string[] RoomEventClasses = ["event-blue", "event-yellow", "event-green"];
    private DateTime CurrentMonth { get; set; } = new(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);

    private DateTime? SelectedDate { get; set; }

    private bool IsEventModalOpen { get; set; }

    private IReadOnlyList<ApiMovie> Movies { get; set; } = Array.Empty<ApiMovie>();

    private IReadOnlyList<ApiRoom> Rooms { get; set; } = Array.Empty<ApiRoom>();

    private IReadOnlyList<MovieEvent> MovieEvents { get; set; } = Array.Empty<MovieEvent>();

    protected PlanningPageViewModel ViewModel { get; private set; } = new();

    [Inject]
    protected IMovieService MovieService { get; set; } = default!;

    [Inject]
    protected IRoomService RoomService { get; set; } = default!;

    [Inject]
    protected IMovieEventService MovieEventService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadPlanningDataAsync();
    }

    protected async Task ChangeMonth(int monthOffset)
    {
        CurrentMonth = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(monthOffset);
        SelectedDate = null;
        IsEventModalOpen = false;
        await LoadPlanningDataAsync();
    }

    protected Task OpenEventModal(DateTime selectedDate)
    {
        SelectedDate = DateTime.SpecifyKind(selectedDate, DateTimeKind.Utc);
        IsEventModalOpen = true;
        RebuildViewModel();

        return Task.CompletedTask;
    }

    protected Task CloseEventModal()
    {
        IsEventModalOpen = false;
        RebuildViewModel();

        return Task.CompletedTask;
    }

    protected async Task SaveEvent(ScheduleMovieEventRequest request)
    {
        await MovieEventService.ScheduleMovieEvent(request);
        IsEventModalOpen = false;
        await LoadPlanningDataAsync();
        StateHasChanged();
    }

    private async Task LoadPlanningDataAsync()
    {
        var moviesTask = MovieService.GetMovies(userRole: "Manager");
        var roomsTask = RoomService.GetRooms();
        var eventsTask = MovieEventService.FindMovieEventsForMonth(CurrentMonth.Month, CurrentMonth.Year);

        await Task.WhenAll(moviesTask, roomsTask, eventsTask);

        Movies = await moviesTask;
        Rooms = (await roomsTask).ToList();
        MovieEvents = (await eventsTask).ToList();
        RebuildViewModel();
        StateHasChanged();
    }

    private void RebuildViewModel()
    {
        RebuildViewModelFromData(Movies, Rooms);
    }

    private void RebuildViewModelFromData(IReadOnlyList<ApiMovie> movies, IReadOnlyList<ApiRoom> rooms)
    {
        var month = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        ViewModel.Days = BuildDays(month, rooms);
        ViewModel.RoomLegends = BuildRoomLegends(rooms);
        ViewModel.EventModal = new EventViewModel
        {
            IsOpen = IsEventModalOpen,
            SelectedDate = SelectedDate,
            SelectedDateLabel = SelectedDate?.ToString("D") ?? "No date selected",
            Rooms = BuildRoomOptions(rooms),
            Movies = BuildMovieOptions(movies),
            Times = TimeItems
        };
        ViewModel.Month = new MonthViewModel
        {
            Month = month,
            DisplayName = month.ToString("MMMM yyyy")
        };
    }

    private static IReadOnlyList<RoomLegendViewModel> BuildRoomLegends(IReadOnlyList<ApiRoom> rooms)
    {
        if (rooms.Count == 0)
        {
            return Array.Empty<RoomLegendViewModel>();
        }

        return rooms
            .Select((room, index) => new RoomLegendViewModel
            {
                Label = room.Name,
                CssClass = RoomLegendClasses[index % RoomLegendClasses.Length]
            })
            .ToArray();
    }

    private static IReadOnlyList<RoomViewModel> BuildRoomOptions(IReadOnlyList<ApiRoom> rooms) =>
        rooms
            .Select(room => new RoomViewModel
            {
                Value = room.Id.ToString("D"),
                Label = room.Name
            })
            .ToArray();

    private static IReadOnlyList<MovieViewModel> BuildMovieOptions(IReadOnlyList<ApiMovie> movies) =>
        movies
            .Select(movie => new MovieViewModel
            {
                Value = movie.Id.ToString("D"),
                Label = movie.Title
            })
            .ToArray();

    private IReadOnlyList<DayViewModel> BuildDays(DateTime month, IReadOnlyList<ApiRoom> rooms)
    {
        var days = new List<DayViewModel>();
        var firstDayOfMonth = new DateTime(month.Year, month.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var firstDayOffset = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;

        for (var index = 0; index < firstDayOffset; index++)
        {
            days.Add(new DayViewModel { IsBlank = true });
        }

        var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);

        for (var day = 1; day <= daysInMonth; day++)
        {
            var currentDate = new DateTime(month.Year, month.Month, day, 0, 0, 0, DateTimeKind.Utc);
            var eventsForDay = MovieEvents
                .Where(e => e.Showtime.UtcDateTime.Date == currentDate.Date)
                .OrderBy(e => e.Room.Name)
                .ThenBy(e => e.Showtime)
                .Select(e => {
                    var roomIndex = rooms.ToList().FindIndex(r => r.Id == e.Room.Id);
                    var cssClass = roomIndex >= 0 
                        ? RoomEventClasses[roomIndex % RoomEventClasses.Length] 
                        : "event-blue";
                    
                    return new CalendarEventViewModel {
                        Title = e.Movie.Title,
                        Showtime = e.Showtime,
                        RoomName = e.Room.Name,
                        CssClass = cssClass
                    };
                })
                .ToList();

            days.Add(new DayViewModel
            {
                Date = currentDate,
                DayNumber = day,
                IsSelected = SelectedDate.HasValue && SelectedDate.Value.Date == currentDate.Date,
                CalendarEvents = eventsForDay
            });
        }

        return days;
    }
}