using System;
using System.ComponentModel.DataAnnotations;

namespace RadzenTemplate.Shared
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        [Range(-100, 100)]
        public float TemperatureC { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        public string Summary { get; set; }

        public float TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public Status Status { get; set; }
    }

    public enum Status
    {
        None,
        Unsure,
        Confident,
        Comfirmed
    }
}
