using TodayMantaRay.Models;

namespace TodayMantaRay.Services;

public interface ICalendarService
{
    Task<List<Event>> GetEventsAsync(DateOnly date, CancellationToken cancellationToken = default);
}
