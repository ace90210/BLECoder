using BLECoder.Blazor.Client.Authentication.Extensions;
using BLECoder.Blazor.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BLECoderTemplate.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazorClientSkorubaIdentity("https://ids.example.com", "example", "example.api", "email");

            builder.Services.AddHttpAuthClient(builder.HostEnvironment.BaseAddress);

            builder.Services.AddLocalStorage();

            await builder.Build().RunAsync();
        }
    }
}
