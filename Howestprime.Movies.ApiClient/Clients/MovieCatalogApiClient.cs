using System.Text.Json;
using Howestprime.Movies.ApiClient.Core;
using Howestprime.Movies.ApiClient.Infrastructure;
using Howestprime.Movies.ApiClient.Requests;
using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Movies.ApiClient.Clients;

public sealed class MovieCatalogApiClient(HttpClient httpClient, JsonSerializerOptions serializerOptions)
    : HttpApiClientBase(httpClient, serializerOptions), IMovieCatalogApiClient
{
    public Task<ApiResult<Created>> RegisterMovieAsync(RegisterMovieRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var message = CreateRequest(HttpMethod.Post, "api/movie-catalog");
        SetJsonBody(message, request);

        return SendForCreatedAsync(message, ct);
    }

    public Task<ApiResult<MovieCollection>> SearchMovieCatalogAsync(SearchMovieCatalogRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        EnsureNotBlank(request.UserRole, nameof(request.UserRole));

        var path = BuildPathWithQuery(
            "api/movie-catalog",
            ("title", request.Title),
            ("genres", request.Genres));

        var message = CreateRequest(HttpMethod.Get, path);
        AddRequiredHeader(message, "x-user-role", request.UserRole);

        return SendForJsonAsync<MovieCollection>(message, ct);
    }

    public Task<ApiResult<Movie>> GetMovieByIdAsync(Guid movieId, CancellationToken ct = default)
    {
        var message = CreateRequest(HttpMethod.Get, $"api/movie-catalog/{movieId:D}");
        AddRequiredHeader(message, "x-user-role", "Manager");
        return SendForJsonAsync<Movie>(message, ct);
    }

    public Task<ApiResult<Created>> UpdateMovieDetailsAsync(UpdateMovieDetailsRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var message = CreateRequest(HttpMethod.Put, $"api/movie-catalog/{request.MovieId:D}");
        var genres = request.Genres
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var actors = request.Actors
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        SetJsonBody(message, new
        {
            title = request.Title,
            description = request.Description,
            posterUrl = request.PosterUrl,
            releaseYear = request.ReleaseYear,
            duration = request.Duration,
            genres,
            actors,
            ageRating = request.AgeRating
        });

        return SendForCreatedAsync(message, ct);
    }
}
