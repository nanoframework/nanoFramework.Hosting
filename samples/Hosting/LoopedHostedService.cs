using System;
using System.Threading;
using System.Diagnostics;

using nanoFramework.Hosting;

namespace Hosting
{
    namespace Hosting
    {
        internal class LoopedHostedService : IHostedService, IDisposable
        {
            private Thread _thread;
            private int executionCount = 0;

            public virtual Thread ExecutionThread() => _thread;

            public void StartAsync()
            {
                Debug.WriteLine("Timed Hosted Service running.");

                _thread = new Thread(() =>
                {
                    var count = Interlocked.Increment(ref executionCount);
                    Debug.WriteLine($"Timed Hosted Service is working. Count: {count}");
                    Thread.Sleep(250);
                });
            }

            public void StopAsync()
            {
                Debug.WriteLine("Timed Hosted Service is stopping.");
            }

            public void Dispose() { }
        }
    }
}