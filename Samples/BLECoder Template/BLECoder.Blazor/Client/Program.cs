using BLECoder.Blazor.Client.Authentication.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BLECoder.Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazorClientSkorubaIdentity("https://ids.example.com", "example", "example.api");

            builder.Services.AddHttpAuthClient(builder.HostEnvironment.BaseAddress);

            await builder.Build().RunAsync();
        }
    }
}
