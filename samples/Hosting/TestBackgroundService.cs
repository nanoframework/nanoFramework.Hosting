using System;
using System.Threading;

using nanoFramework.Hosting;

using Microsoft.Extensions.Logging;

namespace Hosting
{
    internal class TestBackgroundService : BackgroundService
    {
        private readonly ILogger _logger;
    
        public TestBackgroundService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(TestBackgroundService));
        }

        protected override void ExecuteAsync(CancellationToken cancellationToken)
        {            
            cancellationToken.Register(() => _logger.LogInformation("Service is stopping."));

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Attempting to do actions.");
                Thread.Sleep(500);
            }
        }
    }
}