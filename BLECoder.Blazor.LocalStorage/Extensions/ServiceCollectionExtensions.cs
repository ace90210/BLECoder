using LocalStorage.JsonConverters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LocalStorage
{
    public static class ServiceCollectionExtensions
    {
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
