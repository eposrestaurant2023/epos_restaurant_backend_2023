using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using eAdmin.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace eAdmin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddScoped(x => {
                var apiUrl = new Uri(builder.Configuration["apiBaseUrl"]);
                return new HttpClient() { BaseAddress = apiUrl };
            });


            builder.Services.AddMudServices();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredSessionStorage();

            builder.Services.AddSingleton<NotifierService>();
             
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>();

            await builder.Build().RunAsync();
             

    }
    }
}
