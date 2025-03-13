//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Threading;
using nanoFramework.Hosting.UnitTests.Mocks;
using nanoFramework.TestFramework;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class BackgroundServiceTests
    {
        [TestMethod]
        public void Dispose_stops_thread()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var service = new BackgroundServiceMock();

            service.StartAsync(cancellationToken);

            Assert.IsTrue(service.StartAsyncCalled.WaitForEvent());
            Assert.IsTrue(service.ExecuteAsyncCalled.WaitForEvent());

            service.Dispose();

            Assert.IsTrue(service.ExecuteAsyncCompleted.WaitForEvent());
        }

        [TestMethod]
        public void StartAsync_starts_thread()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            using var service = new BackgroundServiceMock();

            service.StartAsync(cancellationToken);

            Assert.IsTrue(service.StartAsyncCalled.WaitForEvent());
            Assert.IsTrue(service.ExecuteAsyncCalled.WaitForEvent());
        }

        [TestMethod]
        public void StopAsync_stops_thread()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var service = new BackgroundServiceMock();

            service.StartAsync(cancellationToken);

            Assert.IsTrue(service.StartAsyncCalled.WaitForEvent());

            service.StopAsync(cancellationToken);

            Assert.IsTrue(service.StopAsyncCalled.WaitForEvent());
            Assert.IsTrue(service.ExecuteAsyncCompleted.WaitForEvent());
        }
    }
}
