using System.Diagnostics;
using Assessment.Shared;
using Shouldly;
using TemperatureService.Shared.Client;
using WeatherSummaryService.Shared.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// For best performance, we use one http client for each external host.

var temperatureHttpClient = new HttpClient();
var url = builder.Configuration.GetValue<string>("ExternalHosts:TemperatureServiceBaseUrl");
url.ShouldNotBeNullOrEmpty("ExternalHosts:TemperatureServiceBaseUrl was missing.");
temperatureHttpClient.BaseAddress = new Uri(url);
//temperatureHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Test");
builder.Services.AddSingleton<ITemperatureServiceClient>(new TemperatureServiceClient(temperatureHttpClient));

var weatherSummaryHttpClient = new HttpClient();
url = builder.Configuration.GetValue<string>("ExternalHosts:WeatherSummaryServiceBaseUrl");
url.ShouldNotBeNullOrEmpty("ExternalHosts:WeatherSummaryServiceBaseUrl was missing.");
weatherSummaryHttpClient.BaseAddress = new Uri(url);
//weatherSummaryHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Test");
builder.Services.AddSingleton<IWeatherSummaryServiceClient>(new WeatherSummaryServiceClient(weatherSummaryHttpClient));

builder.Services.AddCustomExceptionHandling();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();