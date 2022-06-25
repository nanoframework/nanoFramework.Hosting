using System;
using System.Threading;

using nanoFramework.Hosting;

using Microsoft.Extensions.Logging;

namespace Hosting
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private Timer _timer = null;
        private int executionCount = 0;
        private readonly ILogger _logger;
    
        public TimedHostedService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(TestBackgroundService));
        }

        public void StartAsync()
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                $"Timed Hosted Service is working. Count: {count}");
        }

        public void StopAsync()
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer.Change(Timeout.Infinite, 0);

        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}