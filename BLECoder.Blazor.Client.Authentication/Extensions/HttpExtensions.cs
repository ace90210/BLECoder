using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLECoder.Blazor.Client.Authentication.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<T> GetContentJsonAsync<T>(this HttpContent httpContent, T defaultValue = default)
        {
            try
            {
                var responseContentStream = await httpContent.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<T>(responseContentStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, IgnoreNullValues = true });
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static async Task<TValue> GetFromJsonAsync<TValue>(this HttpClient client, string requestUri, Func<HttpResponseMessage, Task> onError, TValue defaultValue = default)
        {
            try
            {
                var response = await client.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.GetContentJsonAsync<TValue>();

                    return content;
                }
                await onError.Invoke(response);
            }
            catch (AccessTokenNotAvailableException)
            {
                var customResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                customResponse.Content = new StringContent("No authorision found.");
                await onError.Invoke(customResponse);
            }
            catch (Exception ex)
            {
                var customResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                customResponse.Content = new StringContent(ex.Message);
                await onError.Invoke(customResponse);
            }

            return defaultValue;
        }
    }
}
