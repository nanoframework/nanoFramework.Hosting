//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

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
        public void AddHostedServiceMustInheritInterfaceIHostedService()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(FakeHostedService));
                
                    Assert.Throws(typeof(ArgumentException),
                        () => services.AddHostedService(typeof(ServiceA)));
                
                }).Build();
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
