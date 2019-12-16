using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceWorkerCronJobDemo.Services
{
    public abstract class CronJobService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly bool _executeOnStart;

        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo, bool executeOnStart)
        {
            _expression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;
            _executeOnStart = executeOnStart;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            var occurrences = _expression.GetOccurrences(DateTimeOffset.Now, DateTimeOffset.Now.AddMonths(1), _timeZoneInfo).ToArray();
            if (occurrences.Any())
            {
                var period = occurrences.Length == 1 ? TimeSpan.FromMilliseconds(-1) : occurrences[1] - occurrences[0];
                var dueTime = _executeOnStart ? TimeSpan.Zero : occurrences[0] - DateTimeOffset.Now;
                _timer = new Timer(async s => await DoWork(), null, dueTime, period);
            }
            return Task.CompletedTask;
        }

        public virtual Task DoWork()
        {
            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }
    }

    public interface IScheduleConfig<T>
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
        bool ExecuteOnStart { get; set; }
    }

    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
        public bool ExecuteOnStart { get; set; } = false;
    }

    public static class ScheduledServiceExtensions
    {
        public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : CronJobService
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }
            var config = new ScheduleConfig<T>();
            options.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<T>();
            return services;
        }
    }
}
