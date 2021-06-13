using BLECoder.Blazor.Client.Http;
using BLECoder.Blazor.Client.Models;
using BLECoder.Blazor.Client.Policy;
using Radzen;
using RadzenTemplate.Shared;
using System.Net.Http;
using System.Threading.Tasks;

namespace RadzenTemplate.Client.Services
{
    public class AuthorisedWeatherForecastService : BaseHttpService
    {
        public AuthorisedWeatherForecastService(HttpClient httpClient, NotificationService notificationService) : base (httpClient)
        {
            retryPolicy = DefaultPolicies.NotificationPolicy(notificationService);
        }

        public async Task<FromJsonResponse<WeatherForecast[]>> GetForecast()
        {
            return await GetFromJsonResponseAsync<WeatherForecast[]>("WeatherForecast/authorised");
        }
    }
}
