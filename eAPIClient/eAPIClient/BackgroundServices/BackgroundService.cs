using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eAPIClient.BackgroundServices
{
    public abstract class BackgroundService : IHostedService
    {
        private Task _executeTask;
       
        private readonly CancellationTokenSource _stoppingCts =  new CancellationTokenSource();
       
        public BackgroundService()
        {
            
        }

       
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executeTask = ExecuteAsync(_stoppingCts.Token);
            if (_executeTask.IsCompleted)
            {
                return _executeTask;
            }
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executeTask == null)
            {
                return ;
            }
            try
            {
                 _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executeTask,Task.Delay(Timeout.Infinite,cancellationToken));
            }
        }

        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           
            do
            {
                await Process();
                await Task.Delay(5000,stoppingToken);
            } while (!stoppingToken.IsCancellationRequested);
        }

        

        protected abstract Task Process();
    }
}
