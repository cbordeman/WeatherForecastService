using System.Diagnostics;
using Shouldly;
using System.Net.Http.Json;
using WeatherSummaryService.Shared.Models;

namespace WeatherSummaryService.Shared.Clients;

public class WeatherSummaryServiceClient : IWeatherSummaryServiceClient
{
    public const string ClientUrlPath = "/weathersummary";

    private readonly HttpClient httpClient;

    public WeatherSummaryServiceClient(HttpClient httpClient)
    {
        httpClient.BaseAddress.ShouldNotBeNull();
        this.httpClient = httpClient;
    }

    public Task<IEnumerable<WeatherSummary>> GetWeatherSummaries()
    {
        return httpClient.GetFromJsonAsync<IEnumerable<WeatherSummary>>(ClientUrlPath);
    }

    public Task AddWeatherSummaries(IEnumerable<WeatherSummary> weatherSummaries)
    {
        return httpClient!.PostAsJsonAsync(ClientUrlPath, weatherSummaries);
    }
}