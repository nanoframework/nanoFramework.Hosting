//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using Microsoft.Extensions.Hosting;

namespace nanoFramework.Hosting.UnitTests.Mocks
{
    public class HostedServiceMock : IHostedService, IHostedServiceMock
    {
        private readonly ManualResetEvent _executeAsyncCalled = new(false);
        private readonly ManualResetEvent _executeAsyncCompleted = new(false);
        private readonly ManualResetEvent _startAsyncCalled = new(false);
        private readonly ManualResetEvent _stopAsyncCalled = new(false);

        private readonly bool _startThrowsException;
        private readonly bool _stopThrowsException;

        public HostedServiceMock(bool startThrowsException = false, bool stopThrowsException = false)
        {
            _startThrowsException = startThrowsException;
            _stopThrowsException = stopThrowsException;
        }

        public WaitHandle ExecuteAsyncCalled => _executeAsyncCalled;
        public WaitHandle ExecuteAsyncCompleted => _executeAsyncCompleted;
        public WaitHandle StartAsyncCalled => _startAsyncCalled;
        public WaitHandle StopAsyncCalled => _stopAsyncCalled;

        public void StartAsync(CancellationToken cancellationToken)
        {
            if (_startThrowsException)
            {
                throw new Exception();
            }

            _startAsyncCalled.Set();

            _executeAsyncCalled.Set();
            _executeAsyncCompleted.Set();
        }

        public void StopAsync(CancellationToken cancellationToken)
        {
            if (_stopThrowsException)
            {
                throw new Exception();
            }

            _stopAsyncCalled.Set();
        }
    }
}
