using RadzenTemplate.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RadzenTemplate.Server.Controllers
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
        private static int forecastCount = 5, calls = 0;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WeatherForecast>> Get()
        {
            Thread.Sleep(500);

            var rng = new Random();
            return Ok(Enumerable.Range(1, forecastCount).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray());
        }

        [HttpGet("authorised")]
        public ActionResult<IEnumerable<WeatherForecast>> GetAuthorised()
        {
            var rng = new Random();

            if (rng.Next(10) < 7)
                return BadRequest("Unlucky");

            return Ok(Enumerable.Range(1, forecastCount * 8).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray());
        }

        [HttpPost]
        public ActionResult<IEnumerable<WeatherForecast>> Post([FromBody] int newCount)
        {
            if (newCount <= 3)
                return BadRequest("Cannot set to count of 3 or less");
            else if (newCount > 20)
                return BadRequest("Cannot set to count of over 20");

            forecastCount = newCount;
            return Get();
        }

        [HttpPut]
        public ActionResult<IEnumerable<WeatherForecast>> Put([FromBody] int increaseCountBy)
        {
            if (forecastCount + increaseCountBy > 20)
                return BadRequest("Cannot set to count of over 20");

            forecastCount += increaseCountBy;
            return Get();
        }

        [HttpDelete("{count}")]
        public ActionResult<int> Delete(int count)
        {
            if (forecastCount - count <= 3)
                return BadRequest("Cannot set to count of 3 or less");

            forecastCount -= count;
            return Ok(forecastCount);
        }
    }
}
