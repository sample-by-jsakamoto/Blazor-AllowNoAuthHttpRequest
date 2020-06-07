using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;

namespace BlazorWasmApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient<AuthorizationMessageHandler>(sp =>
            {
                var provider = sp.GetRequiredService<IAccessTokenProvider>();
                var naviManager = sp.GetRequiredService<NavigationManager>();
                var handler = new AuthorizationMessageHandler(provider, naviManager);
                handler.ConfigureHandler(authorizedUrls: new[] {
                    naviManager.ToAbsoluteUri("authorized/").AbsoluteUri
                });
                return handler;
            });

            builder.Services.AddHttpClient("BlazorWasmApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<AuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorWasmApp.ServerAPI"));

            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
    }
}
