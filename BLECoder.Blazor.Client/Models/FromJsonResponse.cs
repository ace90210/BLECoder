using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BLECoder.Blazor.Client.Models
{
    public class FromJsonResponse<T>
    {
        public T Content { get; set; }

        public HttpResponseMessage FullResponse { get; set; }

        public bool IsSuccessful => FullResponse.IsSuccessStatusCode;

        public string Message { get; set; }

        public Dictionary<string, List<string>> ModelState { get; set; }

        public Exception Exception { get; set; }
    }
}
