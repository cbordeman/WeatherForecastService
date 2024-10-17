using Shouldly;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TemperatureService.Shared.Models;

namespace TemperatureService.Shared.Client;

public class TemperatureServiceClient : ITemperatureServiceClient
{
    public const string ClientUrlPath = "temperature";

    private readonly HttpClient httpClient;

    public TemperatureServiceClient(HttpClient httpClient)
    {
        httpClient.BaseAddress.ShouldNotBeNull();
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TemperatureForecast>> GetTemperatureForecasts()
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<TemperatureForecast>>(ClientUrlPath);        
    }

    public async Task AddTemperatureForecasts(IEnumerable<TemperatureForecast> temperatureForecasts)
    {
        await httpClient.PostAsJsonAsync(ClientUrlPath, temperatureForecasts);
    }
}