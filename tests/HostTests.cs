using System;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.Hosting.UnitTests.Fakes;
using nanoFramework.TestFramework;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class HostTests
    {
        [TestMethod]
        public void ctor_throws_if_services_is_null()
        {
            Assert.ThrowsException(typeof(ArgumentNullException), () => new Internal.Host(null));
        }

        [TestMethod]
        public void Start_starts_IHostedService()
        {
            var service = new FakeHostedService(startThrows: false, stopThrows: false);
            var sut = new HostBuilder()
                .ConfigureServices(services => services.AddSingleton(typeof(IHostedService), service))
                .Build();

            var registeredService = sut.Services.GetRequiredService(typeof(IHostedService)) as FakeHostedService;

            sut.Start();
           
            Assert.IsNotNull(registeredService);
            Assert.IsTrue(registeredService.IsStarted);
            Assert.IsFalse(registeredService.IsStopped);
        }

        [TestMethod]
        public void Start_throws_if_IHostedService_throws()
        {
            var service = new FakeHostedService(startThrows: true, stopThrows: false);
            var sut = new HostBuilder()
                .ConfigureServices(services => services.AddSingleton(typeof(IHostedService), service))
                .Build();

            Assert.ThrowsException(typeof(AggregateException), () => sut.Start());
        }

        [TestMethod]
        public void Stop_stops_IHostedService()
        {
            var service = new FakeHostedService(startThrows: false, stopThrows: false);
            var sut = new HostBuilder()
                .ConfigureServices(services => services.AddSingleton(typeof(IHostedService), service))
                .Build();

            sut.Start();
            sut.Stop();

            var registeredService = sut.Services.GetRequiredService(typeof(IHostedService)) as FakeHostedService;

            Assert.IsNotNull(registeredService);
            Assert.IsTrue(registeredService.IsStarted);
            Assert.IsTrue(registeredService.IsStopped);
        }

        [TestMethod]
        public void Stop_throws_if_IHostedService_throws()
        {
            var service = new FakeHostedService(startThrows: false, stopThrows: true);
            var sut = new HostBuilder()
                .ConfigureServices(services => services.AddSingleton(typeof(IHostedService), service))
                .Build();

            sut.Start();

            Assert.ThrowsException(typeof(AggregateException), () => sut.Stop());
        }
    }
}
