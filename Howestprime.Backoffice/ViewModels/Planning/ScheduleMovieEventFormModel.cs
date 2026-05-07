using System.ComponentModel.DataAnnotations;

namespace Howestprime.Backoffice.ViewModels.Planning;

public class ScheduleMovieEventFormModel
{
    [Required(ErrorMessage = "Room is required")]
    public string RoomId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Time is required")]
    public string Time { get; set; } = string.Empty;

    [Required(ErrorMessage = "Movie is required")]
    public string MovieId { get; set; } = string.Empty;
}

