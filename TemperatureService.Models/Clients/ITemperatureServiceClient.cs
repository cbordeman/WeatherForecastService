using TemperatureService.Shared.Models;

namespace TemperatureService.Shared.Client;

public interface ITemperatureServiceClient
{
    Task<IEnumerable<TemperatureForecast>> GetTemperatureForecasts();
    Task AddTemperatureForecasts(IEnumerable<TemperatureForecast> temperatureForecasts);
}