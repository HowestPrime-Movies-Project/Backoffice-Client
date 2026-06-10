namespace Howestprime.Movies.ApiClient.Requests;

public sealed class UpdateMovieDetailsRequest
{
    public Guid MovieId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string PosterUrl { get; set; } = string.Empty;

    public int ReleaseYear { get; set; }

    public int Duration { get; set; }

    public string Genres { get; set; } = string.Empty;

    public string Actors { get; set; } = string.Empty;

    public int AgeRating { get; set; }
}
