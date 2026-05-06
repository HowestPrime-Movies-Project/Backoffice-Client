using Howestprime.Movies.ApiClient.Clients;
using Howestprime.Movies.ApiClient.Core;
using Howestprime.Movies.ApiClient.Models;
using Howestprime.Movies.ApiClient.Requests;
using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.Services;

public sealed class MovieEventService(IMovieEventsApiClient movieEventsApiClient) : IMovieEventService
{
    public async Task<string> ScheduleMovieEvent(ScheduleMovieEventRequest eventRequest)
    {
        ArgumentNullException.ThrowIfNull(eventRequest);

        eventRequest.Showtime = eventRequest.Showtime.ToUniversalTime();

        Console.WriteLine(eventRequest.Showtime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
        var result = await movieEventsApiClient.ScheduleMovieEventAsync(eventRequest);
        if (result.IsFailure)
        {
            throw new InvalidOperationException(FormatError(result.Error));
        }

        return result.Value?.Location ?? string.Empty;
    }

    public async Task<IList<MovieEvent>> FindMovieEventsForMonth(int month, int year)
    {
        try
        {
            var result = await movieEventsApiClient.FindMovieEventsForMonthAsync(new FindMovieEventsForMonthRequest
            {
                Month = month,
                Year = year
            });

            if (result.IsFailure)
            {
                // Return empty list if API fails
                return [];
            }

            return result.Value?.ToList() ?? [];
        }
        catch
        {
            // Return empty list on any error
            return [];
        }
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
