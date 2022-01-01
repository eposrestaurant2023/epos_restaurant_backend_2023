using eAPIClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace eAPIClient.Services
{
    public class HubConnectionService
    { 
         
        public  HubConnection connection { get; set; }

       public  async Task OnConnectToHub(string file_watcher_path)
        {
            var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();
            string hub_connection = config.GetValue<string>("server_api_url");

            
            connection = new HubConnectionBuilder().WithUrl($"{hub_connection}hub").Build();
            await connection.StartAsync();
            SyncData(file_watcher_path);
        }
        public void SyncData(string path)
        {
            
            connection.On<string>("Sync", data => {
                System.IO.File.Create(Path.Combine(path, $"{data}_{Guid.NewGuid()}.txt"));
            });
            
        }


    }
}
