using System.ComponentModel.DataAnnotations;

namespace Howestprime.Backoffice.ViewModels.Movie;

public class MovieRegistration
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Release year is required")]
    [Range(1900, 2100, ErrorMessage = "Release year must be between 1900 and 2100")]
    public int ReleaseYear { get; set; } = DateTime.Now.Year;

    [Required(ErrorMessage = "Duration is required")]
    [Range(1, 500, ErrorMessage = "Duration must be between 1 and 500 minutes")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "At least one genre is required")]
    public string Genres { get; set; } = string.Empty;

    [Required(ErrorMessage = "At least one actor is required")]
    public string Actors { get; set; } = string.Empty;

    [Required(ErrorMessage = "Age rating is required")]
    [Range(0, 21, ErrorMessage = "Age rating must be between 0 and 21")]
    public int AgeRating { get; set; }

    [Required(ErrorMessage = "Poster URL is required")]
    [Url(ErrorMessage = "Poster URL must be a valid URL")]
    public string PosterUrl { get; set; } = string.Empty;
}