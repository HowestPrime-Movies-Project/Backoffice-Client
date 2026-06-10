using Howestprime.Backoffice.Services;
using Howestprime.Backoffice.ViewModels.Movie;
using Howestprime.Movies.ApiClient.Requests;
using Microsoft.AspNetCore.Components;

namespace Howestprime.Backoffice.Components.Pages;

public partial class MovieDetails
{
    [Parameter]
    public Guid Id { get; set; }

    private MovieRegistration EditModel { get; set; } = new();
    private string? SuccessMessage { get; set; }
    private string? ErrorMessage { get; set; }
    private bool IsSubmitting { get; set; }
    private bool IsLoading { get; set; } = true;
    private bool NotFound { get; set; }

    [Inject]
    protected IMovieService MovieService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadMovieAsync();
    }

    private async Task LoadMovieAsync()
    {
        IsLoading = true;
        var movie = await MovieService.GetMovieById(Id);

        if (movie is null)
        {
            NotFound = true;
            IsLoading = false;
            return;
        }

        EditModel = new MovieRegistration
        {
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            Duration = movie.Duration,
            Genres = string.Join(", ", movie.Genres),
            Actors = string.Join(", ", movie.Actors),
            AgeRating = movie.AgeRating,
            PosterUrl = movie.PosterUrl
        };

        IsLoading = false;
    }

    private async Task HandleValidSubmit()
    {
        IsSubmitting = true;
        ErrorMessage = null;
        SuccessMessage = null;

        try
        {
            var request = new UpdateMovieDetailsRequest
            {
                MovieId = Id,
                Title = EditModel.Title,
                Description = EditModel.Description,
                PosterUrl = EditModel.PosterUrl,
                ReleaseYear = EditModel.ReleaseYear,
                Duration = EditModel.Duration,
                Genres = EditModel.Genres,
                Actors = EditModel.Actors,
                AgeRating = EditModel.AgeRating
            };

            await MovieService.UpdateMovieDetails(request);
            SuccessMessage = $"'{EditModel.Title}' updated successfully!";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to update movie: {ex.Message}";
            Console.WriteLine($"Error updating movie: {ex}");
        }
        finally
        {
            IsSubmitting = false;
            StateHasChanged();
        }
    }

    private Task HandlePosterUrlChanged()
    {
        StateHasChanged();
        return Task.CompletedTask;
    }
}
