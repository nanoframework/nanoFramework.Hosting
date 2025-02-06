using Microsoft.Extensions.Hosting;
using System;
using System.Threading;

namespace nanoFramework.Hosting.UnitTests.Mocks
{
    internal class MockBackgroundService: BackgroundService, IMockHostedService
    {
        private readonly ManualResetEvent _executeAsyncCalled = new(false);
        private readonly ManualResetEvent _executeAsyncCompleted = new(false);
        private readonly ManualResetEvent _startAsyncCalled = new(false);
        private readonly ManualResetEvent _stopAsyncCalled = new(false);

        private readonly bool _startThrowsException;
        private readonly bool _stopThrowsException;

        public MockBackgroundService(bool startThrowsException = false, bool stopThrowsException = false)
        {
            _startThrowsException = startThrowsException;
            _stopThrowsException = stopThrowsException;
        }

        public WaitHandle ExecuteAsyncCalled => _executeAsyncCalled;
        public WaitHandle ExecuteAsyncCompleted => _executeAsyncCompleted;
        public WaitHandle StartAsyncCalled => _startAsyncCalled;
        public WaitHandle StopAsyncCalled => _stopAsyncCalled;

        protected override void ExecuteAsync(CancellationToken stoppingToken)
        {
            _executeAsyncCalled.Set();

            while (!stoppingToken.IsCancellationRequested)
            {
                TestHelper.Sleep();
            }

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
