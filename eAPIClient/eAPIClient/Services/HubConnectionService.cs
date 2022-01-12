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
using Polly;
using Serilog;

namespace eAPIClient.Services
{
    public class HubConnectionService
    { 
         
        public  HubConnection connection { get; set; }
        private readonly ISyncService sync;
        public HubConnectionService()
        {

        }
        public HubConnectionService(ISyncService sync)
        {
            this.sync = sync;
        }
       public  async Task OnConnectToHub()
        {
          
            var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();
            string hub_connection = config.GetValue<string>("server_api_url");

            
            connection = new HubConnectionBuilder().WithUrl($"{hub_connection}hub").WithAutomaticReconnect().Build();
            
            await OpenSignalRConnection();

        

             SyncData();
        }


        public async Task OpenSignalRConnection()
        {
            try
            {
                await connection.StartAsync();
            }catch{
                
                Log.Information("Connect to hub fail");
            }
        }
         


        public void  SyncData()
        {
            
            connection.On<string>("Sync", async data => {
                switch (data)
                {
                    case "all": //sync all data
                        await sync.SyncAllData();
                        break;
                    case "setting":
                        await sync.SyncSetting();
                        break;
                    default:
                        break;
                }
            });

         


            

        }

 

    }
}
