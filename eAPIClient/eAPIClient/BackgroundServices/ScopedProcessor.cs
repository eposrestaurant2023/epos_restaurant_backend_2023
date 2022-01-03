using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eAPIClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace eAPIClient.BackgroundServices
{
    public abstract class ScopedProcessor : BackgroundService
    {
        private IServiceScopeFactory _serviceScopeFactory;
        public ScopedProcessor(IServiceScopeFactory serviceScopeFactory) :base()
        {
           
            _serviceScopeFactory = serviceScopeFactory;
           
        }
        protected override async Task Process()
        {
            var scope = _serviceScopeFactory.CreateScope();

            await ProcessInScope(scope.ServiceProvider);

        }

        

        public abstract Task ProcessInScope(IServiceProvider scopeServiceProvider);
       


    }
}
