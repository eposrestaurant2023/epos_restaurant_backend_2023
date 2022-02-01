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
            //string upload = env.ContentRootPath + "\\uploads";  
            //if (!Directory.Exists(upload))
            //{
            //    Directory.CreateDirectory(upload);
            //}

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


            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;


            //watch for sync remote data

            var watcherRemoteData = new FileSystemWatcher(path);

            watcherRemoteData.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;


            watcherRemoteData.Created += sync.OnSyncFromRemoteServerAsync;


            watcherRemoteData.Filter = "*.bat";
            watcherRemoteData.IncludeSubdirectories = true;
            watcherRemoteData.EnableRaisingEvents = true;



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
