using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Services
{
    public class HubConnectionService
    {
        public async Task ConnectToService()
        {
            HubConnection connection = new HubConnectionBuilder().WithUrl("http://localhost:7073/api/connectionhub").Build();
            await connection.StartAsync();
        }
    }
}
