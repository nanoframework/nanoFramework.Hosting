//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;

using nanoFramework.TestFramework;
using nanoFramework.Hosting.UnitTests.Fakes;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class SchedulerServiceTests
    {
        [TestMethod]
        public void StartStopAndDisposeSchedulerService()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(FakeSchedulerService));
                }).Build();

            var service = (FakeSchedulerService)host.Services.GetService(typeof(IHostedService));
            Assert.NotNull(service);

            host.Start();
            Assert.True(service.IsStarted);

            Thread.Sleep(20);
            Assert.True(service.IsCompleted);

            host.Stop();
            Assert.True(service.IsStopped);

            host.Dispose();
        }

        // TODO: This isn't a SchedulerService test. It's a HostBuilder test and it's already covered.
        [TestMethod]
        public void StartStopSchedulerServiceThrowsAggregateException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(ExecptionSchedulerService));
                }).Build();

            Assert.Throws(typeof(AggregateException),
                () => host.Start());

            Assert.Throws(typeof(AggregateException),
                () => host.Stop());
        }
    }
}
