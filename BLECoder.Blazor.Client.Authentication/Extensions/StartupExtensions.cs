using BLECoder.Blazor.Client.Authentication.Security;
using BLECoder.Blazor.Client.Authentication.Tools;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BLECoder.Blazor.Client.Authentication.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddBlazorClientSkorubaIdentity(this IServiceCollection services, string authority, string clientId, string apiScope)
        {
            services.AddOidcAuthentication(options =>
            {
                options.ProviderOptions.Authority = authority;

                options.ProviderOptions.ClientId = clientId;

                options.ProviderOptions.ResponseType = "code";

                options.UserOptions.RoleClaim = "role";

                options.ProviderOptions.DefaultScopes.Add("roles");
                options.ProviderOptions.DefaultScopes.Add(apiScope);
            }).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            return services;
        }

        public static void AddHttpAuthClient(this IServiceCollection services, string baseAddress)
        {
            services.AddHttpClient("DefaultClient", client => client.BaseAddress = new Uri(baseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            services.AddHttpClient("NoAuthenticationClient", client => client.BaseAddress = new Uri(baseAddress));

            services.AddScoped<HttpAuthClient>();
        }
    }
}
