using System;
using System.Net.Http;

namespace BLECoder.Blazor.Client.Models
{
    public class FromJsonResponse<T>
    {
        public T Content { get; set; }

        public HttpResponseMessage FullResponse { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
