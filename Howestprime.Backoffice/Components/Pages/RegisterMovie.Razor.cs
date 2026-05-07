using Howestprime.Backoffice.Services;
using Howestprime.Backoffice.ViewModels.Movie;
using Howestprime.Movies.ApiClient.Requests;
using Microsoft.AspNetCore.Components;

namespace Howestprime.Backoffice.Components.Pages;

public partial class RegisterMovie
{
    private MovieRegistration RegistrationModel { get; set; } = new();
    private string? SuccessMessage { get; set; }
    private string? ErrorMessage { get; set; }
    private bool IsSubmitting { get; set; }

    [Inject]
    protected IMovieService MovieService { get; set; } = default!;

    private async Task HandleValidSubmit()
    {
        IsSubmitting = true;
        ErrorMessage = null;
        SuccessMessage = null;

        try
        {
            var request = new RegisterMovieRequest
            {
                Title = RegistrationModel.Title,
                Description = RegistrationModel.Description,
                ReleaseYear = RegistrationModel.ReleaseYear,
                Duration = RegistrationModel.Duration,
                Genres = RegistrationModel.Genres.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
                Actors = RegistrationModel.Actors.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
                AgeRating = RegistrationModel.AgeRating,
                PosterUrl = RegistrationModel.PosterUrl
            };

            var movieId = await MovieService.RegisterMovie(request);
            SuccessMessage = $"Movie '{RegistrationModel.Title}' registered successfully! (ID: {movieId})";
            ClearForm();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to register movie: {ex.Message}";
            Console.WriteLine($"Error registering movie: {ex}");
        }
        finally
        {
            IsSubmitting = false;
            StateHasChanged();
        }
    }

    private Task HandlePosterUrlChanged()
    {
        // Trigger re-render when poster URL changes so preview updates
        StateHasChanged();
        return Task.CompletedTask;
    }

    private void ClearForm()
    {
        RegistrationModel = new MovieRegistration();
    }
}