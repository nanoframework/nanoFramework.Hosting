//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Threading;

namespace nanoFramework.Hosting.UnitTests.Fakes
{
    public class FakeBackgroundService : BackgroundService
    {
        public bool IsStarted { get; private set; }
        public bool IsStopped { get; private set; }

        public ManualResetEvent StartedEvent { get; } = new(false);
        public ManualResetEvent StoppedEvent { get; } = new(false);

        protected override void ExecuteAsync()
        {
            IsStarted = true;
            StartedEvent.Set();

            while (!CancellationRequested)
            {
                Thread.Sleep(10);
            }

            IsStopped = true;
            StoppedEvent.Set();
        }
    }
}
