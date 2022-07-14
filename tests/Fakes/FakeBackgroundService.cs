//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Threading;

namespace nanoFramework.Hosting.UnitTests.Fakes
{
    public class FakeBackgroundService : BackgroundService
    {
        public bool IsStarted { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsStopped { get; set; }

        public override void Start()
        {
            IsStarted = true;

            base.Start();
        }

        protected override void ExecuteAsync()
        {
            IsCompleted = true;

            while (!CancellationRequested)
            {
                Thread.Sleep(10);
            }
        }

        public override void Stop()
        {
            IsStopped = true;

            base.Stop();
        }
    }
}
