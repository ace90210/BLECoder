using IdentityModel.AspNetCore.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace BLECoder.Blazor.Server.Authentication.Extensions
{
    public static class StartupExtensions
    {
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
    }
}
