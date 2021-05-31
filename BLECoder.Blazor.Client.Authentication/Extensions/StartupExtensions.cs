using BLECoder.Blazor.Client.Authentication.Security;
using BLECoder.Blazor.Client.Authentication.Tools;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BLECoder.Blazor.Client.Authentication.Extensions
{
    /// <summary>
    /// Startup Helper extensions for registering services.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Registers the Oidc provider for authentication, providing suitable defaults to map and request roles and inject role mapping for Blazor Client.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="authority">The Authority Address</param>
        /// <param name="clientId">The Client Id</param>
        /// <param name="apiScope">The Api Scope</param>
        /// <param name="additionalScopes">(Optional): Any additional scopes to request.</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddBlazorClientSkorubaIdentity(this IServiceCollection services, string authority, string clientId, string apiScope, params string[] additionalScopes)
        {
            services.AddOidcAuthentication(options =>
            {
                options.ProviderOptions.Authority = authority;

                options.ProviderOptions.ClientId = clientId;

                options.ProviderOptions.ResponseType = "code";

                options.UserOptions.RoleClaim = "role";

                options.ProviderOptions.DefaultScopes.Add("roles");
                options.ProviderOptions.DefaultScopes.Add(apiScope);

                if(additionalScopes != null && additionalScopes.Length > 0)
                {
                    foreach(var scope in additionalScopes)
                    {
                        options.ProviderOptions.DefaultScopes.Add(scope);
                    }
                }

            }).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            return services;
        }

        /// <summary>
        /// Registers the Oidc provider for authentication, providing suitable defaults to map and request roles and inject role mapping for Blazor Client.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="options">The Oidc provider options to use.</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddBlazorClientSkorubaIdentity(this IServiceCollection services, OidcProviderOptions options)
        {
            services.AddOidcAuthentication(opt =>
            {
                opt.ProviderOptions.Authority = options.Authority;
                opt.ProviderOptions.MetadataUrl = options.MetadataUrl;
                opt.ProviderOptions.RedirectUri = options.RedirectUri;
                opt.ProviderOptions.ResponseMode = options.ResponseMode;
                opt.ProviderOptions.ResponseType = options.ResponseType ?? "code";
                opt.ProviderOptions.ClientId = options.ClientId;
                opt.ProviderOptions.PostLogoutRedirectUri = options.PostLogoutRedirectUri;

                if(options.DefaultScopes?.Count > 0)
                {
                    foreach(var scope in options.DefaultScopes)
                    {
                        if (!opt.ProviderOptions.DefaultScopes.Contains(scope))
                        {
                            opt.ProviderOptions.DefaultScopes.Add(scope);
                        }
                    }
                }

                if (!opt.ProviderOptions.DefaultScopes.Contains("roles"))
                {
                    opt.ProviderOptions.DefaultScopes.Add("roles");
                }

                opt.UserOptions.RoleClaim = "role";

            }).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            return services;
        }

        /// <summary>
        /// Register the HttpAuthClient and its dependencies
        /// </summary>
        /// <param name="services">The startup service collection</param>
        /// <param name="baseAddress">The base address of all calls (usually "builder.HostEnvironment.BaseAddress" is best)</param>
        public static void AddHttpAuthClient(this IServiceCollection services, string baseAddress)
        {
            services.AddHttpClient("DefaultClient", client => client.BaseAddress = new Uri(baseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            services.AddHttpClient("NoAuthenticationClient", client => client.BaseAddress = new Uri(baseAddress));

            services.AddScoped<HttpAuthClient>();
        }
    }
}
