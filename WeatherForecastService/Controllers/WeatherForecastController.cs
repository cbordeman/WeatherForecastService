using Microsoft.AspNetCore.Mvc;
using Shouldly;
using TemperatureService.Shared.Client;
using TemperatureService.Shared.Models;
using WeatherSummaryService.Shared.Clients;
using WeatherSummaryService.Shared.Models;

namespace FrontEndService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITemperatureServiceClient temperatureClient;
        private readonly IWeatherSummaryServiceClient weatherSummaryClient;

        public WeatherForecastController(ITemperatureServiceClient temperatureClient,
            IWeatherSummaryServiceClient weatherSummaryClient,
            ILogger<WeatherForecastController> logger)
        {
            this.temperatureClient = temperatureClient;
            this.weatherSummaryClient = weatherSummaryClient;
            _logger = logger;
        }

        [HttpGet]
        public async ValueTask<IEnumerable<WeatherForecast>> Get()
        {
            var temperatureTask = temperatureClient.GetTemperatureForecasts();
            var summaryTask = weatherSummaryClient.GetWeatherSummaries();
            await Task.WhenAll(temperatureTask, summaryTask);

            // Combine temps and summaries
            var leftOuterJoin = from a in temperatureTask.Result
                                join b in summaryTask.Result
                                on a.Date equals b.Date into temp
                                from b in temp.DefaultIfEmpty()
                                select new WeatherForecast(a.Date, a?.TemperatureC, b?.Summary);
            var rightOuterJoin = from b in summaryTask.Result
                                 join a in temperatureTask.Result
                                 on b.Date equals a.Date into temp
                                 from a in temp.DefaultIfEmpty()
                                 select new WeatherForecast(b.Date, a?.TemperatureC, b?.Summary);
            var fullOuterJoin = leftOuterJoin.Union(rightOuterJoin);

            var combined = fullOuterJoin.ToArray();

            return combined;
        }

        [HttpPost]
        public async Task Post(IEnumerable<WeatherForecast> weatherForecasts)
        {
            weatherForecasts.ShouldAllBe(x => x.TemperatureC != null && x.Summary != null);

            var temperatureTask = temperatureClient.AddTemperatureForecasts(
                weatherForecasts.Select(wf => new TemperatureForecast(wf.Date, wf.TemperatureC ?? 0)));
            var summaryTask = weatherSummaryClient.AddWeatherSummaries(
                weatherForecasts.Select(wf => new WeatherSummary(wf.Date, wf.Summary!)));
            await Task.WhenAll(temperatureTask, summaryTask);
        }
    }
}
