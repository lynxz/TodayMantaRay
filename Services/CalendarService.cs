using System.Text;
using TodayMantaRay.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TodayMantaRay.Services;

internal class CalendarService : ICalendarService
{
    private const string FILE = "/config/calendar.yml";
    private readonly ILogger<CalendarService> _logger;
    private readonly IDeserializer _deserializer;

    public CalendarService(ILogger<CalendarService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        _logger.LogDebug($"Constructed {nameof(CalendarService)}");
    }

    public async Task<List<Event>> GetEventsAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"Executing {nameof(GetEventsAsync)} with date ${date}");
        try
        {
            var yml = await File.ReadAllTextAsync(FILE, Encoding.UTF8, cancellationToken);
            var calendar = _deserializer.Deserialize<Calendar>(yml);

            List<Event> events = new();
            events.AddRange(calendar.Birthdays.Where(b => IsOnDate(date, b)));
            events.AddRange(calendar.Holidays.Where(h => IsOnDate(date, h)));

            return events;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Deserialization failed!");
            return new List<Event>();
        }
    }

    private bool IsOnDate(DateOnly date, Event @event) =>
        @event switch
        {
            Birthday b => b.Date?.Day == date.Day && b.Date?.Month == date.Month,
            Holiday h => h.End == null ?
             h.Start?.Day == date.Day && h.Start?.Month == date.Month :
             (h.Start?.Day >= date.Day && h.Start?.Month == date.Month) && (h.End?.Day <= date.Day && h.End?.Month <= h.End?.Month),
            { } => false,
            null => false
        };
}