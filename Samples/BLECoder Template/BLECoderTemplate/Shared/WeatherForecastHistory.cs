using System;

namespace BLECoderTemplate.Shared
{
    public class WeatherForecastHistory
    {
        public DateTime TimeOfForecast { get; set; } = DateTime.Now;

        public bool WasAuth { get; set; }

        public WeatherForecast[] Forecasts { get; set; }
    }
}
