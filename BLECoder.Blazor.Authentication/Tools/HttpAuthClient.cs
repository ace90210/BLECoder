using System.Net.Http;

namespace BLECoder.Blazor.Authentication.Tools
{
    public class HttpAuthClient : HttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private HttpClient client;
        private HttpClient authClient;

        public HttpAuthClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient Client(bool requireAuth)
        {
            if (requireAuth)
            {
                if (authClient == null)
                    authClient = _httpClientFactory.CreateClient("DefaultClient");

                return authClient;
            }
            else
            {
                if (client == null)
                    client = _httpClientFactory.CreateClient("NoAuthenticationClient");

                return client;
            }
        }
    }
}
