using BLECoder.Blazor.LocalStorage.JsonConverters;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BLECoder.Blazor.LocalStorage
{
    /// <summary>
    /// Extensions for the service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register local storage with a <see cref="JsonStringEnumConverter"/> and <see cref="TimespanJsonConverter"/> converter.
        /// Provides <see cref="ILocalStorageServiceAsync"/> and <see cref="ILocalStorageServiceSync"/> for both syncronous and asyncronous local storage injection.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddLocalStorage(this IServiceCollection services)
        {
            return services
                .AddScoped(context =>
                {
                    var options = new JsonSerializerOptions();

                    options.Converters.Add(new JsonStringEnumConverter());
                    options.Converters.Add(new TimespanJsonConverter());
                    return options;
                })
                .AddScoped<ILocalStorageServiceAsync, LocalStorageServiceAsync>()
                .AddScoped<ILocalStorageServiceSync, LocalStorageServiceSync>();
        }
    }
}
