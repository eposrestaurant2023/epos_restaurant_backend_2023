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
        private readonly IHttpService http;
        private readonly IConfiguration config;
        public HubConnectionService(IHttpService _http, IConfiguration _conf)
        {
            http = _http;
            config = _conf;
        }
        public  HubConnection connection { get; set; }

       public  async Task OnConnectToHub()
        {
            string hub_connection = config.GetValue<string>("server_api_url");
            connection = new HubConnectionBuilder().WithUrl($"{hub_connection}hub").Build();
            await connection.StartAsync();
            Fetch();
        }
        public void Fetch()
        {
            
            connection.On<string>("Sync", data => {
                switch (data)
                {
                    case "setting":
                            
                    default:
                        break;
                }
            });
            
        }


    }
}
