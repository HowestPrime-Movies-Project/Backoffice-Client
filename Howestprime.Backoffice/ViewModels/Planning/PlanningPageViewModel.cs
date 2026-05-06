namespace Howestprime.Backoffice.ViewModels.Planning;

public class PlanningPageViewModel
{
    public MonthViewModel Month { get; set; } = new();

    public IReadOnlyList<RoomLegendViewModel> RoomLegends { get; set; } = Array.Empty<RoomLegendViewModel>();

    public IReadOnlyList<DayViewModel> Days { get; set; } = Array.Empty<DayViewModel>();

    public EventViewModel EventModal { get; set; } = new();
}
