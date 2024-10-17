using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using TemperatureService.Shared.Models;

namespace TemperatureService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemperatureController(ILogger<TemperatureController> logger) : ControllerBase
    {
        private static readonly ConcurrentDictionary<DateOnly, TemperatureForecast> TemperatureForecasts = new();

        static TemperatureController()
        {
            for (var i = 0; i < 4; i++)
            {
                var date = DateOnly.FromDateTime(DateTime.Today.AddDays(i));
                var tf = new TemperatureForecast(date, i * 15);
                TemperatureForecasts.TryAdd(date, tf);
            }
        }

        private readonly ILogger<TemperatureController> _logger = logger;

        [HttpGet]
        public async ValueTask<IEnumerable<TemperatureForecast>> Get()
        {
            await Task.Delay(Random.Shared.Next(3000));
            return TemperatureForecasts.Values.OrderBy(x => x.Date);
        }

        [HttpPost]
        public async ValueTask Post(IEnumerable<TemperatureForecast> temperatureForecasts)
        {
            await Task.Delay(Random.Shared.Next(3000));
            foreach (var tf in temperatureForecasts)
                TemperatureForecasts.AddOrUpdate(tf.Date, tf, 
                    (_, tf2) => tf2 with { TemperatureC = tf.TemperatureC });
        }
    }
}