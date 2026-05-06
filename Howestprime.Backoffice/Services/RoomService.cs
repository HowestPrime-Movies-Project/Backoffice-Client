using Howestprime.Movies.ApiClient.Clients;
using Howestprime.Movies.ApiClient.Requests;
using Howestprime.Movies.ApiClient.Responses;

namespace Howestprime.Backoffice.Services;

public sealed class RoomService(IMovieEventsApiClient movieEventsApiClient) : IRoomService
{
    private static readonly Room BlueRoom = new() { Id = Guid.Parse("019d059e-d220-71db-8a1a-ec7569492999"), Name = "Blue Room", Capacity = 100};
    private static readonly Room YellowRoom = new() { Id = Guid.Parse("019d059e-d220-75fe-b936-0a97cd75216e"), Name = "Yellow Room", Capacity = 150};

    public async Task<IList<Room>> GetRooms()
    {
        var roomsById = new Dictionary<Guid, Room>
        {
            [BlueRoom.Id] = BlueRoom,
            [YellowRoom.Id] = YellowRoom
        };
        var referenceMonth = new DateTimeOffset(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, TimeSpan.Zero);

        foreach (var (month, year) in GetMonthsToInspect(referenceMonth))
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
                    // Log/skip this month but don't fail completely
                    continue;
                }

                foreach (var movieEvent in result.Value ?? [])
                {
                    if (movieEvent?.Room != null)
                    {
                        roomsById[movieEvent.Room.Id] = movieEvent.Room;
                    }
                }
            }
            catch
            {
                // If this month fails, skip it and try the next
                continue;
            }
        }

        // Return discovered rooms, or empty list if none found
        return roomsById.Values
            .OrderBy(room => room.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static IEnumerable<(int Month, int Year)> GetMonthsToInspect(DateTimeOffset referenceMonth)
    {
        var start = new DateTime(referenceMonth.Year, referenceMonth.Month, 1).AddMonths(-1);
        for (var index = 0; index < 3; index++)
        {
            var current = start.AddMonths(index);
            yield return (current.Month, current.Year);
        }
    }
}
