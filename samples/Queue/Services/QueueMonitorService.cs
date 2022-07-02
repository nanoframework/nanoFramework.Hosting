//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;

using nanoFramework.Hosting;

namespace Hosting
{
    internal class QueueMonitorService : SchedulerService
    {
        private readonly BackgroundQueue _queue;

        public QueueMonitorService(BackgroundQueue queue)
            : base(TimeSpan.FromSeconds(1))
        {
            _queue = queue;
        }

        public override void StartAsync()
        {
            Debug.WriteLine($"Service '{nameof(QueueMonitorService)}' is now running in the background.");

            base.StartAsync();
        }

        protected override void ExecuteAsync(object state)
        {
            Debug.WriteLine($"Queue Depth: {_queue.QueueCount}");
        }

        public override void StopAsync()
        {
            Debug.WriteLine($"Service '{nameof(QueueMonitorService)}' is stopping.");
            
            base.StopAsync();
        }
    }
}