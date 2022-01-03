using eAPIClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eAPIClient.BackgroundServices
{
    public abstract class SceduledProcessor : ScopedProcessor
    {

        public CrontabSchedule _schedule;
        private DateTime _nextRun;
        protected abstract string Schedule { get; }
        public SceduledProcessor(IServiceScopeFactory serviceScopeFactory) :base(serviceScopeFactory)
        {
            _schedule = CrontabSchedule.Parse(Schedule);
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                if (now > _nextRun)
                {
                    await Process();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken);
            } while (!stoppingToken.IsCancellationRequested);
        }
        public abstract override Task<Task> ProcessInScope(IServiceProvider scopeServiceProvider);
       
    }
}
