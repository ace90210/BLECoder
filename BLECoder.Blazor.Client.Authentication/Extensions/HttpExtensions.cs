using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLECoder.Blazor.Client.Authentication.Extensions
{
    /// <summary>
    /// Http Helper Extensions to simplify Http Requests and reading of response contents.
    /// </summary>
    public static class HttpExtensions
    {
        public static JsonSerializerOptions DefaultSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, IgnoreNullValues = true };

        /// <summary>
        /// Return the content of as the specific object.
        /// </summary>
        /// <typeparam name="T">The data type to return the data in</typeparam>
        /// <param name="httpContent">The HttpContent</param>
        /// <param name="defaultValue">The default value to provide if there is an error.</param>
        /// <returns>The content deserialized.</returns>
        public static async Task<T> GetContentJsonAsync<T>(this HttpContent httpContent, T defaultValue = default)
        {
            try
            {
                var responseContentStream = await httpContent.ReadAsStreamAsync();

                if(DefaultSerializerOptions != null)
                    return await JsonSerializer.DeserializeAsync<T>(responseContentStream, DefaultSerializerOptions);


                return await JsonSerializer.DeserializeAsync<T>(responseContentStream);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the value resulting from deserialize the response body as JSON in an asynchronous operation. 
        /// If a non success response is returned or error occurs the onError function is called and provided the <see cref="HttpResponseMessage"/> returned from the call, or the error wrapped in a <see cref="HttpResponseMessage"/>
        /// </summary>
        /// <typeparam name="TValue">The type of the object to deserialize to and return.</typeparam>
        /// <param name="client">The client used to send the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="onError">The event called when a non success or error occurs</param>
        /// <param name="defaultValue">The default value to provide if there is an error.</param>
        /// <returns>The response deserialized.</returns>
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
