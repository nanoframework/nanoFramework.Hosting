using System;
using System.Threading;

using nanoFramework.TestFramework;
using nanoFramework.Hosting.UnitTests.Fakes;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class BackgroundServiceTests
    {
        [TestMethod]
        public void StartStopAndDisposeBackgroundService()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(FakeBackgroundService));
                }).Build();

            var service = (FakeBackgroundService)host.Services.GetService(typeof(IHostedService));
            Assert.NotNull(service);

            host.Start();
            Assert.True(service.IsStarted);

            Thread.Sleep(10);
            Assert.True(service.IsCompleted);

            host.Stop();
            Assert.True(service.IsStopped);

            host.Dispose();
        }

        [TestMethod]
        public void StartStopBackgroundServiceThrowsAggregateException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(ExecptionBackgroundService));
                }).Build();

                Assert.Throws(typeof(AggregateException),
                    () => host.Start());

                Assert.Throws(typeof(AggregateException),
                    () => host.Stop());
        }

    }
}
