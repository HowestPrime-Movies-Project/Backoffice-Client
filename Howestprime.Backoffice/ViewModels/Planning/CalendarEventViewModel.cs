namespace Howestprime.Backoffice.ViewModels.Planning;

public class CalendarEventViewModel
{
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset Showtime { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string CssClass { get; set; } = string.Empty;
}
