using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadzenTemplate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RadzenTemplate.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaultyController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private static int counter = 0;

        public FaultyController()
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<WeatherForecast>> Get()
        {
            if (++counter % 4 == 0)
            {
                var rng = new Random();
                return Ok(Enumerable.Range(1, 40).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray());
            }
            else if (counter % 4 == 1)
            {
                return StatusCode(501);
            }
            else if (counter % 4 == 2)
            {
                return StatusCode(502);
            }
            else
            {
                return StatusCode(503);
            }

        }
    }
}
