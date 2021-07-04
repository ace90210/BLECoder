using Polly;
using Polly.Extensions.Http;
using Radzen;
using System;
using System.Net.Http;

namespace BLECoder.Blazor.Client.Policy
{
    public static class DefaultPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> NotificationPolicy(NotificationService notificationService, Func<int, TimeSpan> sleepDurationProvider, int retryLimit = 3, int notificationDurationMs = 2500)
        {
            return HttpPolicyExtensions
                                .HandleTransientHttpError()
                                .WaitAndRetryAsync(retryLimit, sleepDurationProvider, onRetry: (response, retryDelay, retryCount, context) =>
                                {

                                    if (retryCount == retryLimit)
                                    {
                                        notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Failed Request after {retryCount} tries. Reason: {response?.Result?.StatusCode}.", Duration = notificationDurationMs });
                                    }
                                    else
                                    {
                                        notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Warning", Detail = $"Failed Request retrying. Tries={retryCount}. Reason: {response?.Result?.StatusCode}.", Duration = notificationDurationMs });
                                    }
                                });
        }

        public static IAsyncPolicy<HttpResponseMessage> NotificationPolicy(NotificationService notificationService, float baseWaitSeconds = 1.5f, int retryLimit = 3, int notificationDurationMs = 2500)
        {
            return NotificationPolicy(notificationService, retryAttempt => TimeSpan.FromSeconds(baseWaitSeconds * retryAttempt), retryLimit, notificationDurationMs);
        }
    }
}
