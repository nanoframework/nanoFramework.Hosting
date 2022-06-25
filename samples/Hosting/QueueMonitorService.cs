using System;
using System.Threading;
using System.Diagnostics;

using nanoFramework.Hosting;

namespace Hosting
{
    internal class QueueMonitorService : IHostedService, IDisposable
    {
        private Timer _timer = null;
        private readonly BackgroundQueue _queue;

        public QueueMonitorService(BackgroundQueue queue)
        {
            _queue = queue;
        }

        public void StartAsync()
        {
            Debug.WriteLine($"Service '{nameof(QueueMonitorService)}' is now running in the background.");
            _timer = new Timer(GetQueueCount, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private void GetQueueCount(object state)
        {
            Debug.WriteLine($"Queue Depth: {_queue.QueueCount}");
        }

        public void StopAsync()
        {
            Debug.WriteLine($"Service '{nameof(QueueMonitorService)}' is stopping.");
            _timer.Change(Timeout.Infinite, 0);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}