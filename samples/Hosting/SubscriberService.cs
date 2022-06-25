using System;
using System.Threading;
using System.Diagnostics;

using nanoFramework.Hosting;

namespace Hosting
{
    internal class SubscriberService : BackgroundService
    {
        private readonly BackgroundQueue _queue;

        public SubscriberService(BackgroundQueue queue)
        {
            _queue = queue;
        }

        protected override void ExecuteAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Service '{nameof(SubscriberService)}' is now running in the background.");
            cancellationToken.Register(() => Debug.WriteLine($"Service '{nameof(SubscriberService)}' is stopping."));

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Thread.Sleep(50);

                    var workItem = _queue.Dequeue();
                    if (workItem == null)
                    {
                        continue;
                    }

                    //Debug.WriteLine($"{workItem} found!");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An error occurred when dequeueing work item. Exception: {ex}");
                }
            }
        }
    }
}
