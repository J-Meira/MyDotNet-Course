using Microsoft.AspNetCore.Mvc;
namespace MyDotNetAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
  private readonly string[] _summaries = new[]
  {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  };

  [HttpGet("/{forecastCount}", Name = "GetWeatherForecast")]
  public IEnumerable<WeatherForecast> GetForecast(int forecastCount)
  {
    var forecast = Enumerable.Range(1, forecastCount).Select(index =>
      new WeatherForecast
      (
          DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
          Random.Shared.Next(-20, 55),
          _summaries[Random.Shared.Next(_summaries.Length)]
      ))
      .ToArray();
    return forecast;
  }

}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

