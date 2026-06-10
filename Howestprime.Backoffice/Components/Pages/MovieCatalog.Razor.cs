using Howestprime.Backoffice.Services;
using Howestprime.Movies.ApiClient.Responses;
using Microsoft.AspNetCore.Components;

namespace Howestprime.Backoffice.Components.Pages;

public partial class MovieCatalog
{
    private IReadOnlyList<Movie> Movies { get; set; } = [];
    private bool IsLoading { get; set; } = true;

    [Inject]
    protected IMovieService MovieService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Movies = await MovieService.GetMovies(userRole: "Manager");
        IsLoading = false;
    }
}
