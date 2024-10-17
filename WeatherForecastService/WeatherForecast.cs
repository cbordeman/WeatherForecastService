namespace FrontEndService;

public record WeatherForecast(
    DateOnly Date,
    int? TemperatureC,
    string? Summary)
{
    public int? TemperatureF => TemperatureC == null ? null : 32 + (int)(TemperatureC / 0.5556);
}