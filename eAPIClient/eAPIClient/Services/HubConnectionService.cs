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
    public static class HubConnectionService
    {
        public static HubConnection connection { get; set; }

       public static async Task OnConnectToHub()
        {
            var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();
            string hub_connection = config.GetValue<string>("server_api_url");
            connection = new HubConnectionBuilder().WithUrl($"{hub_connection}connectionhub").Build();
            await connection.StartAsync();
            Fetch();
        }
        public static void Fetch()
        {
            
            connection.On<CustomerModel>("Sync", m => {
                string path = @$"c:\deleteme\FileCreatedFromClient{Guid.NewGuid()}.json";
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(JsonSerializer.Serialize(m));
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            });
            
        }
    }
}
