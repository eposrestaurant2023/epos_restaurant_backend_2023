using eAPI.BackgroundServices;
using eAPI.Services;
using eModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace eAPI.ScheduleTasks
{
    public class EposRestaurantTask : SceduledProcessor
    {
        public EposRestaurantTask(IServiceScopeFactory serviceScpoeFactory) : base(serviceScpoeFactory)
        {

        }

        protected override string Schedule => "*/1 * * * *"; // */1mn  *h *DoM *M *DoW

        public override async Task<Task> ProcessInScope(IServiceProvider scopeServiceProvider)
        {

            var scope = scopeServiceProvider.CreateScope();

            var service = scopeServiceProvider.GetRequiredService<BackendSyncService>();
            //var db = scopeServiceProvider.GetRequiredService<ApplicationDbContext>();

           await service.CheckSystemFeatures();
           return Task.CompletedTask;
        }
    }
}
