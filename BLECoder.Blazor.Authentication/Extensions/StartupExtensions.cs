using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using IdentityModel.AspNetCore.AccessTokenValidation;
using BLECoder.Blazor.Authentication.Security;
using BLECoder.Blazor.Authentication.Tools;

namespace BLECoder.Blazor.Authentication.Extensions
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

                options.ProviderOptions.DefaultScopes.Add("openid");
                options.ProviderOptions.DefaultScopes.Add("profile");
                options.ProviderOptions.DefaultScopes.Add("roles");
                options.ProviderOptions.DefaultScopes.Add(apiScope);
            }).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            return services;
        }

        public static IServiceCollection AddBlazorServerAuthentication(this IServiceCollection services, string authority, string resourceId, bool enableIntrospectionEndpoint = false, string resourceSecret = null)
        {
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = authority;

                options.Audience = resourceId;

                if (enableIntrospectionEndpoint)
                {
                    // if token does not contain a dot, it is a reference token
                    options.ForwardDefaultSelector = Selector.ForwardReferenceToken("introspection");
                }
            });

            if (enableIntrospectionEndpoint)
            {
                // reference tokens
                authenticationBuilder.AddOAuth2Introspection("introspection", options =>
                {
                    options.Authority = authority;

                    options.ClientId = resourceId;
                    options.ClientSecret = resourceSecret;
                });
            }
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
