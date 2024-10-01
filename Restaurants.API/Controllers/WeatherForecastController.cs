using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;

namespace Restaurants.API.Controllers;

public class TemperatureRequest
{
    public int Min { get; set; }
    public int Max { get; set; }
}

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }
    
    [HttpGet]
    [Route("/weatherforecast")]
    public  IEnumerable<WeatherForecast> Get()
    {
        var result = _weatherForecastService.Get(2, 2, 4);
        return result;
    }

    [HttpPost("generate")]
    public IActionResult Generate([FromQuery]int count, [FromBody] TemperatureRequest request)
    {
        if (count < 0 || request.Max < request.Min)
        {
            return BadRequest("Count has to be positive number, and max must be greater than or equal to min");
        }
        var result = _weatherForecastService.Get(count, request.Min, request.Max);
        return Ok(result);
        
    }
}