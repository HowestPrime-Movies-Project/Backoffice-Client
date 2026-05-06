using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.ViewModels.Planning;

public class DayViewModel
{
    public DateTime? Date { get; set; }

    public int? DayNumber { get; set; }

    public bool IsBlank { get; set; }

    public bool IsSelected { get; set; }

    public IReadOnlyList<MovieEvent> Events { get; set; } = Array.Empty<MovieEvent>();

    public IReadOnlyList<CalendarEventViewModel> CalendarEvents { get; set; } = Array.Empty<CalendarEventViewModel>();
}