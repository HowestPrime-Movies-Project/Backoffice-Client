namespace Howestprime.Backoffice.ViewModels.Planning;

public class EventViewModel
{
    public bool IsOpen { get; set; }

    public DateTime? SelectedDate { get; set; }

    public string SelectedDateLabel { get; set; } = string.Empty;

    public IReadOnlyList<RoomViewModel> Rooms { get; set; } = Array.Empty<RoomViewModel>();

    public IReadOnlyList<MovieViewModel> Movies { get; set; } = Array.Empty<MovieViewModel>();

    public IReadOnlyList<string> Times { get; set; } = Array.Empty<string>();
}