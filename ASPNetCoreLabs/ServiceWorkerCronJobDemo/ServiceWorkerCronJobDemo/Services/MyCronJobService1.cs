using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ServiceWorkerCronJobDemo.Services
{
    public class MyCronJobService1 : CronJobService
    {
        private readonly ILogger<MyCronJobService1> _logger;

        public MyCronJobService1(IScheduleConfig<MyCronJobService1> config, ILogger<MyCronJobService1> logger)
            : base(config.CronExpression, config.TimeZoneInfo, config.ExecuteOnStart)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CronJobService 1 starts at {DateTime.Now:G}.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork()
        {
            _logger.LogInformation($"CronJobService 1 is working at time: {DateTime.Now:G}");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJobService 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
