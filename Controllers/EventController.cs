using Microsoft.AspNetCore.Mvc;
using TodayMantaRay.Services;

namespace TodayMantaRay.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly ILogger<EventController> _logger;
    private readonly ICalendarService _calendarService;

    public EventController(ILogger<EventController> logger, ICalendarService calendarService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        _logger.LogDebug($"Constructed ${nameof(EventController)}");
    }


    [HttpGet("")]
    public async Task<ICollection<String>> Today()
    {
         _logger.LogDebug($"Executing {nameof(Today)}");
        var events = await _calendarService.GetEventsAsync(DateOnly.FromDateTime(DateTime.Now));
        return events.Select(e => e.ToString(DateTime.Now.Year)).ToList();
    }

    [HttpGet("{date}")]
    public async Task<ICollection<String>> AtDate(DateTime date)
    {
        _logger.LogDebug($"Executing {nameof(AtDate)} with date ${date}");
        var events = await _calendarService.GetEventsAsync(DateOnly.FromDateTime(date));
        return events.Select(e => e.ToString(date.Year)).ToList();
    }

}