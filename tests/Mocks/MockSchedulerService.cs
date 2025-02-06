//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using Microsoft.Extensions.Hosting;

namespace nanoFramework.Hosting.UnitTests.Mocks
{
    public class MockSchedulerService : SchedulerService, IMockHostedService
    {
        private readonly ManualResetEvent _executeAsyncCalled = new(false);
        private readonly ManualResetEvent _executeAsyncCompleted = new(false);
        private readonly ManualResetEvent _startAsyncCalled = new(false);
        private readonly ManualResetEvent _stopAsyncCalled = new(false);

        private readonly bool _startThrowsException;
        private readonly bool _stopThrowsException;

        public MockSchedulerService(bool startThrowsException = false, bool stopThrowsException = false): base(TestHelper.SleepDelay)
        {
            _startThrowsException = startThrowsException;
            _stopThrowsException = stopThrowsException;
        }

        public WaitHandle ExecuteAsyncCalled => _executeAsyncCalled;
        public WaitHandle ExecuteAsyncCompleted => _executeAsyncCompleted;
        public WaitHandle StartAsyncCalled => _startAsyncCalled;
        public WaitHandle StopAsyncCalled => _stopAsyncCalled;

        public int Executions { get; private set; }

        protected override void ExecuteAsync(CancellationToken stoppingToken)
        {
            _executeAsyncCalled.Set();
          
            Executions++;

            _executeAsyncCompleted.Set();
        }

        public override void StartAsync(CancellationToken cancellationToken)
        {
            if (_startThrowsException)
            {
                throw new Exception();
            }

            base.StartAsync(cancellationToken);

            _startAsyncCalled.Set();
        }

        public override void StopAsync(CancellationToken cancellationToken)
        {
            if (_stopThrowsException)
            {
                throw new Exception();
            }

            base.StopAsync(cancellationToken);

            _stopAsyncCalled.Set();
        }
    }
}
