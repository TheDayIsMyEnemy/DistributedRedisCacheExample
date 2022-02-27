using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedRedisCache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDistributedCacheService _distributedCacheService;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IDistributedCacheService distributedCacheService)
        {
            _logger = logger;
            _distributedCacheService = distributedCacheService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            string key = nameof(WeatherForecast);

            var cachedWeatherForecast = await _distributedCacheService.GetAsync<IEnumerable<WeatherForecast>>(key);
            if (cachedWeatherForecast != null)
            {
                return cachedWeatherForecast;
            }

            var weatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

            await _distributedCacheService
                .SetAsync(key, weatherForecast,
            new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(10) } );

            return weatherForecast;
        }
    }
}