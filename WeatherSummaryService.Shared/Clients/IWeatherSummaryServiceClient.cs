using WeatherSummaryService.Shared.Models;

namespace WeatherSummaryService.Shared.Clients;

public interface IWeatherSummaryServiceClient
{
    Task<IEnumerable<WeatherSummary>> GetWeatherSummaries();
    Task AddWeatherSummaries(IEnumerable<WeatherSummary> weatherSummaries);
}