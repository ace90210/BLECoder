using IdentityModel.AspNetCore.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace BLECoder.Blazor.Server.Authentication.Extensions
{
    /// <summary>
    /// Startup Helper extensions for registering services.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Register Authentication with the specified configuration. Can be either access or reference tokens.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="authority">The Authority to use</param>
        /// <param name="audienceAndApiResourceId">The Audience to validate. If configured as a reference token this is also the API Resource Id</param>
        /// <param name="resourceSecret">(Optional): The API Resource Secret (If specified, reference tokens will be configured.)</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddBlazorServerAuthentication(this IServiceCollection services, string authority, string audienceAndApiResourceId, string resourceSecret = null)
        {
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = authority;

                options.Audience = audienceAndApiResourceId;

                if (!string.IsNullOrWhiteSpace(resourceSecret))
                {
                    // if token does not contain a dot, it is a reference token
                    options.ForwardDefaultSelector = Selector.ForwardReferenceToken("introspection");
                }
            });

            if (!string.IsNullOrWhiteSpace(resourceSecret))
            {
                // reference tokens
                authenticationBuilder.AddOAuth2Introspection("introspection", options =>
                {
                    options.Authority = authority;

                    options.ClientId = audienceAndApiResourceId;
                    options.ClientSecret = resourceSecret;
                });
            }
            return services;
        }

        /// <summary>
        /// Register Authentication with the specified configuration. For reference tokens only.
        /// </summary> 
        /// <param name="services">The service collection</param>
        /// <param name="authority">The Authority to use</param>
        /// <param name="audience">The Audience to validate</param>
        /// <param name="apiResourceId">The API Resource Id</param>
        /// <param name="resourceSecret">The API Resource Secret</param>
        /// <returns></returns>
        public static IServiceCollection AddBlazorServerAuthentication(this IServiceCollection services, string authority, string audience, string apiResourceId, string resourceSecret)
        {
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = authority;

                options.Audience = audience;

                // if token does not contain a dot, it is a reference token
                options.ForwardDefaultSelector = Selector.ForwardReferenceToken("introspection");
            });

            // reference tokens
            authenticationBuilder.AddOAuth2Introspection("introspection", options =>
            {
                options.Authority = authority;

                options.ClientId = apiResourceId;
                options.ClientSecret = resourceSecret;
            });
            return services;
        }
    }
}
