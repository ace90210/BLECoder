using BLECoder.Blazor.Client.Models;
using System;

namespace RadzenTemplate.Client.Models
{
    public interface IErrorComponent
    {
        void ShowError(string title, string message, Exception ex = null);

        void HandleFailedResponse<T>(FromJsonResponse<T> response, string title = "Error");
    }
}
