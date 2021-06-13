using BLECoder.Blazor.Client.Http;
using BLECoder.Blazor.Client.Models;
using BLECoder.Blazor.Client.Policy;
using Radzen;
using RadzenTemplate.Shared;
using System.Net.Http;
using System.Threading.Tasks;

namespace RadzenTemplate.Client.Services
{
    public class HttpWeatherForecastService  : BaseHttpService
    {
        public HttpWeatherForecastService(HttpClient httpClient, NotificationService notificationService) : base(httpClient) 
        {
            retryPolicy = DefaultPolicies.NotificationPolicy(notificationService);
        }

        public async Task<FromJsonResponse<WeatherForecast[]>> GetFaultyForecast()
        {
            return await GetFromJsonResponseAsync<WeatherForecast[]>("Faulty");
        }

        public async Task<FromJsonResponse<WeatherForecast[]>> GetForecast()
        {         
            return await GetFromJsonResponseAsync<WeatherForecast[]>("WeatherForecast");
        }

        public async Task<int> DeleteTest(int count)
        {
            var jsonResposne = await DeleteWithJsonResponseAsync<int>("WeatherForecast/" + count);

            return jsonResposne.Content;
        }

        public async Task<FromJsonResponse<WeatherForecast[]>> PostForecast(int count)
        {
            return await PostFromJsonResponseAsync<WeatherForecast[], int>("WeatherForecast", count);
        }

        public async Task<FromJsonResponse<WeatherForecast[]>> PutForecast(int count)
        {
            return await PutFromJsonResponseAsync<WeatherForecast[], int>("WeatherForecast", count);
        }
    }
}
