namespace Howestprime.Movies.ApiClient.Responses;

public sealed class MovieEventCollection
{
    public IReadOnlyList<Guid> MovieIds { get; set; } = [];
    public IReadOnlyList<MovieEvent> MovieEvents { get; set; } = [];
}

