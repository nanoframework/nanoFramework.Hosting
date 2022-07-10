using System;

using nanoFramework.TestFramework;
using nanoFramework.Hosting.UnitTests.Fakes;


namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class HostedServiceTests
    {
        [TestMethod]
        public void StartStopHostedService()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(FakeHostedService));
                }).Build();

            var service = (FakeHostedService)host.Services.GetService(typeof(IHostedService));
            Assert.NotNull(service);

            host.Start();
            Assert.True(service.IsStarted);

            host.Stop();
            Assert.True(service.IsStopped);
        }

        [TestMethod]
        public void StartStopHostedServiceThrowsAggregateException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(ExceptionHostedService));
                }).Build();

            Assert.Throws(typeof(AggregateException),
                () => host.Start());

            Assert.Throws(typeof(AggregateException),
                () => host.Stop());
        }

        [TestMethod]
        public void StopWithoutStartNoops()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(FakeHostedService));
                }).Build();

            host.Stop();
        }

        [TestMethod]
        public void StartWhenAlreadyStartedNoops()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(FakeHostedService));
                }).Build();

            host.Start();
            host.Start();
        }
    }
}
