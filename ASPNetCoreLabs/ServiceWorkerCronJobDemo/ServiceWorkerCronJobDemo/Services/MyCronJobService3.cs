using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ServiceWorkerCronJobDemo.Services
{
    public class MyCronJobService3 : CronJobService
    {
        private readonly ILogger<MyCronJobService3> _logger;

        public MyCronJobService3(IScheduleConfig<MyCronJobService3> config, ILogger<MyCronJobService3> logger)
            : base(config.CronExpression, config.TimeZoneInfo, config.ExecuteOnStart)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CronJobService 3 starts at {DateTime.Now:G}.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork()
        {
            _logger.LogInformation($"CronJobService 3 is working at time: {DateTime.Now:G}");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJobService 3 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
