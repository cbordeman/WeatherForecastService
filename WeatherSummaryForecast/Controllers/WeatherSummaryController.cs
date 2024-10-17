using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using WeatherSummaryService.Shared.Models;

namespace WeatherSummaryForecastService.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherSummaryController(ILogger<WeatherSummaryController> logger) : ControllerBase
{
    private static readonly ConcurrentDictionary<DateOnly, WeatherSummary> Summaries = new();

    static WeatherSummaryController()
    {
        for (int i = 0; i < 4; i++)
        {
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(i));
            Summaries.TryAdd(date, new WeatherSummary(date, i switch
            {
                0 => "Freezing",
                1 => "Warm",
                2 => "Hot",
                3 => "Scorched Earth"
            }));            
        }
    }

    private readonly ILogger<WeatherSummaryController> _logger = logger;

    [HttpGet]
    public async ValueTask<IEnumerable<WeatherSummary>> Get()
    {
        await Task.Delay(Random.Shared.Next(3000));
        return Summaries.Values.OrderBy(x => x.Date);
    }

    [HttpPost]
    public async ValueTask Post(IEnumerable<WeatherSummary> forecastSummaries)
    {
        await Task.Delay(Random.Shared.Next(3000));
        foreach (var fs in forecastSummaries)
            Summaries.AddOrUpdate(fs.Date, fs, 
                (_, fs2) => fs2 with { Summary = fs.Summary });
    }
}