using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eAdmin.Services;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MatBlazor;
using MudBlazor.Services;
using eAdmin.Shared.Extensions;
 
using MudBlazor;

namespace eAdmin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddLocalization();
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddScoped(x => {
                var apiUrl = new Uri(builder.Configuration["apiBaseUrl"]);
                return new HttpClient() { BaseAddress = apiUrl };
            });

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;

                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 5000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });



            builder.Services.AddMatBlazor();
    


            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddOptions();
            builder.Services.AddSingleton<NotifierService>();


            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>();



            var host = builder.Build();
            await host.SetDefaultCulture();
            await host.RunAsync();
        }
    }
}
