using BLECoder.Blazor.Client.Authentication.Extensions;
using BLECoder.Blazor.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using RadzenTemplate.Client.Models.Radzen;
using RadzenTemplate.Client.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RadzenTemplate.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazorClientSkorubaIdentity("https://ids.runeclawgames.com", "example", "example.api", "email");

            builder.Services.AddHttpAuthClient(builder.HostEnvironment.BaseAddress);

            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();

            builder.Services.AddLocalStorage();

            builder.Services.AddScoped<ThemeState>();

            builder.Services.AddHttpClient<AuthorisedWeatherForecastService>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<HttpWeatherForecastService>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            await builder.Build().RunAsync();
        }
    }
}
