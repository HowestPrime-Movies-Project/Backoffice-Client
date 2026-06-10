using Howestprime.Movies.ApiClient.Core;
using Howestprime.Movies.ApiClient.Requests;
using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Movies.ApiClient.Clients;

public interface IMovieCatalogApiClient
{
    Task<ApiResult<Created>> RegisterMovieAsync(RegisterMovieRequest request, CancellationToken ct = default);

    Task<ApiResult<MovieCollection>> SearchMovieCatalogAsync(SearchMovieCatalogRequest request, CancellationToken ct = default);

    Task<ApiResult<Movie>> GetMovieByIdAsync(Guid movieId, CancellationToken ct = default);

    Task<ApiResult<Created>> UpdateMovieDetailsAsync(UpdateMovieDetailsRequest request, CancellationToken ct = default);
}
