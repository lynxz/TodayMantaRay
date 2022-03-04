using TodayMantaRay.Models;

namespace TodayMantaRay.Services;

public class SingerHostedService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public SingerHostedService(IServiceProvider services, ILogger<SingerHostedService> logger)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Singer Hosted Service running.");
        await Run(stoppingToken);
    }

    private async Task Run(CancellationToken stoppingToken)
    {
        bool hasRunToday = false;

        while (!stoppingToken.IsCancellationRequested)
        {
            if (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 3)
            {
                hasRunToday = false;
            }
            if (DateTime.Now.Hour > 6 && !hasRunToday)
            {
                _logger.LogInformation("Singing for those with birthdays!");
                hasRunToday = true;
                var birthdays = await GetBirthdaysAsync(stoppingToken);
                foreach(var birthday in birthdays)
                    await SingAsync(birthday, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Singer Hosted Service is stopping.");
        return base.StopAsync(cancellationToken);
    }

    private async Task<IList<Birthday?>> GetBirthdaysAsync(CancellationToken stoppingToken)
    {
        using (var scope = _services.CreateScope())
        {
            var calendarService = scope.ServiceProvider.GetRequiredService<ICalendarService>();
            var events = await calendarService.GetEventsAsync(DateOnly.FromDateTime(DateTime.Now), stoppingToken);
            return events.Where(e => e is Birthday).Select(e => e as Birthday).ToList();
        }
    }

    private async Task SingAsync(Birthday? birthday, CancellationToken stoppingToken) {
        _logger.LogInformation("Happy Birthday to You");
        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        _logger.LogInformation("Happy Birthday to You");
        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        _logger.LogInformation($"Happy Birthday Dear {birthday?.Name}");
        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        _logger.LogInformation("Happy Birthday to You");
    }

}