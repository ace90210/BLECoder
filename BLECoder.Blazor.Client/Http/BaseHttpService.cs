using BLECoder.Blazor.Client.Authentication.Extensions;
using BLECoder.Blazor.Client.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Polly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLECoder.Blazor.Client.Http
{
    public abstract class BaseHttpService
    {
        protected readonly HttpClient httpClient;

        public event Func<HttpResponseMessage, Task> OnErrorAsync;

        public event Func<Task<bool>> OnDeleteRequestAsync;

        public IAsyncPolicy<HttpResponseMessage> retryPolicy;

        public BaseHttpService(HttpClient httpClient) { this.httpClient = httpClient; }

        protected async Task<FromJsonResponse<TValue>> GetFromJsonResponseAsync<TValue>(string requestUri, bool expectModelStateOnBadRequest = true)
        {
            return await SendFromJsonResponseAsync<TValue>(() => httpClient.GetAsync(requestUri), expectModelStateOnBadRequest);
        }

        protected async Task<FromJsonResponse<TResultValue>> PostFromJsonResponseAsync<TResultValue, TInputValue>(string requestUri, TInputValue value, bool expectModelStateOnBadRequest = true)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            using var stringContent = new StringContent(serializedValue, Encoding.UTF8, "application/json");

            return await SendFromJsonResponseAsync<TResultValue>(() => httpClient.PostAsync(requestUri, stringContent), expectModelStateOnBadRequest);
        }

        protected async Task<FromJsonResponse<TValue>> PostFromJsonResponseAsync<TValue>(string requestUri, TValue value, bool expectModelStateOnBadRequest = true)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            using var stringContent = new StringContent(serializedValue, Encoding.UTF8, "application/json");

            return await SendFromJsonResponseAsync<TValue>(() => httpClient.PostAsync(requestUri, stringContent), expectModelStateOnBadRequest);
        }

        protected async Task<FromJsonResponse<TResultValue>> PutFromJsonResponseAsync<TResultValue, TInputValue>(string requestUri, TInputValue value, bool expectModelStateOnBadRequest = true)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            using var stringContent = new StringContent(serializedValue, Encoding.UTF8, "application/json");

            return await SendFromJsonResponseAsync<TResultValue>(() => httpClient.PutAsync(requestUri, stringContent), expectModelStateOnBadRequest);
        }

        protected async Task<FromJsonResponse<TValue>> PutFromJsonResponseAsync<TValue>(string requestUri, TValue value, bool expectModelStateOnBadRequest = true)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            using var stringContent = new StringContent(serializedValue, Encoding.UTF8, "application/json");

            return await SendFromJsonResponseAsync<TValue>(() => httpClient.PutAsync(requestUri, stringContent), expectModelStateOnBadRequest);
        }

        protected async Task<FromJsonResponse<TValue>> DeleteWithJsonResponseAsync<TValue>(string requestUri, bool expectModelStateOnBadRequest = true)
        {
            var delete = await ProcessOnDelete();
            if (delete)
                return await SendFromJsonResponseAsync<TValue>(() => httpClient.DeleteAsync(requestUri), expectModelStateOnBadRequest);

            return new FromJsonResponse<TValue>() { Message = "User Cancelled Delete" };
        }

        private async Task<FromJsonResponse<TValue>> SendFromJsonResponseAsync<TValue>(Func<Task<HttpResponseMessage>> action, bool expectModelStateOnBadRequest = true)
        {
            var fromJsonResponse = new FromJsonResponse<TValue>();
            try
            {
                HttpResponseMessage response = null;

                if (retryPolicy != null)
                    response = await retryPolicy.ExecuteAsync(action);
                else
                    response = await action.Invoke();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.GetContentJsonAsync<TValue>();

                    fromJsonResponse.Content = content;
                    fromJsonResponse.FullResponse = response;
                    fromJsonResponse.Message = "Success";
                }
                else
                {
                    await ProcessOnError(response);

                    fromJsonResponse.FullResponse = response;

                    if (expectModelStateOnBadRequest)
                    {
                        try
                        {
                            var modelState = await response.Content?.GetContentJsonAsync<Dictionary<string, List<string>>>();

                            fromJsonResponse.ModelState = modelState;
                        }
                        catch (Exception)
                        {
                            var customResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                            customResponse.Content = new StringContent("No model state found.");

                            fromJsonResponse.FullResponse = customResponse;
                            fromJsonResponse.Message = "No model state found.";
                        }
                    }
                }
            }
            catch (AccessTokenNotAvailableException)
            {
                var customResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                customResponse.Content = new StringContent("No authorision found.");

                await ProcessOnError(customResponse);

                fromJsonResponse.FullResponse = customResponse;
                fromJsonResponse.Message = "No authorision found";
            }
            catch (Exception ex)
            {
                var customResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                customResponse.Content = new StringContent(ex.Message);

                await ProcessOnError(customResponse);

                fromJsonResponse.FullResponse = customResponse;
                fromJsonResponse.Message = "Unexpected error occured";
                fromJsonResponse.Exception = ex;
            }

            return fromJsonResponse;
        }

        protected virtual async Task ProcessOnError(HttpResponseMessage responseMessage)
        {
            if (OnErrorAsync != null)
                await OnErrorAsync.Invoke(responseMessage);
        }

        protected virtual async Task<bool> ProcessOnDelete()
        {
            if (OnDeleteRequestAsync != null)
                return await OnDeleteRequestAsync.Invoke();

            return true;
        }
    }
}
