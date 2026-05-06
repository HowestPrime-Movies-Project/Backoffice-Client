using Howestprime.Movies.ApiClient.Clients;
using Howestprime.Movies.ApiClient.Core;
using Howestprime.Movies.ApiClient.Models;
using Howestprime.Movies.ApiClient.Requests;
using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.Services;

public sealed class MovieService(IMovieCatalogApiClient movieCatalogApiClient) : IMovieService
{
    public async Task<List<Movie>> GetMovies(string? title = null, string? genre = null, string? userRole = null)
    {
        try
        {
            var request = new SearchMovieCatalogRequest
            {
                UserRole = string.IsNullOrWhiteSpace(userRole) ? "Manager" : userRole,
                Title = title,
                Genres = genre
            };

            var result = await movieCatalogApiClient.SearchMovieCatalogAsync(request);
            if (result.IsFailure)
            {
                // Return empty list if API fails instead of throwing
                return [];
            }

            return result.Value?.Data.ToList() ?? [];
        }
        catch
        {
            // Return empty list on any error
            return [];
        }
    }

    public async Task<string> RegisterMovie(RegisterMovieRequest movieRequest)
    {
        ArgumentNullException.ThrowIfNull(movieRequest);

        var result = await movieCatalogApiClient.RegisterMovieAsync(movieRequest);
        if (result.IsFailure)
        {
            throw new InvalidOperationException(FormatError(result.Error));
        }

        return result.Value?.Location ?? string.Empty;
    }

    private static string FormatError(ApiErrorResponse? error)
    {
        if (error is null)
        {
            return "The API request failed.";
        }

        var message = error.Detail ?? error.Title ?? "The API request failed.";
        if (error.Errors is { Count: > 0 })
        {
            var formattedErrors = error.Errors.GetFormattedErrors();
            if (!string.IsNullOrWhiteSpace(formattedErrors))
            {
                message = message + " (" + formattedErrors + ")";
            }
        }

        return message;
    }
}



