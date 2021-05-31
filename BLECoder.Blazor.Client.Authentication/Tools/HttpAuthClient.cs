using System;
using System.Net.Http;

namespace BLECoder.Blazor.Client.Authentication.Tools
{
    /// <summary>
    /// Http Client handler, uses a <see cref="IHttpClientFactory"/> to provide either a stanard client or a client which checks for access tokens and throws a <see cref="AccessTokenNotAvailableException"/> if not found. 
    /// </summary>
    public class HttpAuthClient : IDisposable
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private HttpClient client;
        private HttpClient authClient;

        /// <summary>
        /// Default constructor. 
        /// </summary>
        /// <param name="httpClientFactory">The Http Client Factory to be used for creation for HttpClients. Must provide two clients. "DefaultClient" for authenticated requests and "NoAuthenticationClient" for standard requests.</param>
        public HttpAuthClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Returns the HttpClient. If requireAuth is true the client returned is configured for authenticated requests.
        /// </summary>
        /// <param name="requireAuth">True if authentication may be required.</param>
        /// <returns>The HttpClient</returns>
        /// <remarks>Clients are created on first use and reused after that.</remarks>
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

        /// <summary>
        /// Disposes any clients created
        /// </summary>
        public void Dispose()
        {
            if (client != null)
                client.Dispose();

            if (authClient != null)
                authClient.Dispose();
        }
    }
}
