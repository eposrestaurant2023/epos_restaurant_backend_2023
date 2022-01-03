using eAPIClient.BackgroundServices;
using eAPIClient.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace eAPIClient.ScheduleTasks
{
    public class EposRestaurantTask : SceduledProcessor
    {
        HubConnectionService hub = new HubConnectionService();
        public EposRestaurantTask(IServiceScopeFactory serviceScpoeFactory) : base(serviceScpoeFactory)
        {

        }

        protected override string Schedule => "*/1 * * * *"; // */1mn  *h *DoM *M *DoW

        public override async Task<Task> ProcessInScope(IServiceProvider scopeServiceProvider)
        {

            var scope = scopeServiceProvider.CreateScope();

            var sync = scopeServiceProvider.GetRequiredService<ISyncService>();

            //var db = scopeServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (hub.connection is null)
            {
                hub = new HubConnectionService(sync);
                await hub.OnConnectToHub();
            }
            else
            {

                if (hub.connection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Disconnected)
                {
                    
                    await hub.OpenSignalRConnection();
                }
            }
            return Task.CompletedTask;
        }
    }
}
