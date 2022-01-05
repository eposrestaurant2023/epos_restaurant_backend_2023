using eAPIClient.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAPIClient
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            var host = builder.Build();


            var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;


            var sync = services.GetRequiredService<ISyncService>();
            var env = services.GetRequiredService<IWebHostEnvironment>();

            string path = env.ContentRootPath + "\\logs";
          
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var watcher = new FileSystemWatcher(path);


            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;


            watcher.Created += sync.OnCreatedAsync;


            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;



             

            host.Run();
             
        }
 


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });



    }
}
