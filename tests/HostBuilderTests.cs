using Microsoft.Extensions.DependencyInjection;
using nanoFramework.TestFramework;
using System;
using nanoFramework.Hosting.UnitTests.Fakes;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class HostBuilderTests
    {
        [TestMethod]
        public void Build_and_Dispose()
        {
            var sut = new HostBuilder();
            using var host = sut.Build();

            Assert.IsNotNull(host);
        }

        [TestMethod]
        public void Build_registers_IHost()
        {
            var sut = new HostBuilder();
            using var host = sut.Build();

            Assert.IsNotNull(host.Services.GetRequiredService(typeof(IHost)));
        }

        [TestMethod]
        public void Build_registers_IHostBuilder()
        {
            var sut = new HostBuilder();
            using var host = sut.Build();

            Assert.IsNotNull(host.Services.GetRequiredService(typeof(IHostBuilder)));
        }

        [TestMethod]
        public void Build_registers_IServiceProvider()
        {
            var sut = new HostBuilder();
            using var host = sut.Build();

            Assert.IsNotNull(host.Services.GetRequiredService(typeof(IServiceProvider)));
        }

        [TestMethod]
        public void Build_throws_if_called_more_than_once()
        {
            var sut = new HostBuilder();
            using var host = sut.Build();

            Assert.IsNotNull(host);
            Assert.ThrowsException(typeof(InvalidOperationException), () => sut.Build());
        }

        [TestMethod]
        public void ConfigureServices_can_be_called_multiple_times()
        {
            var callCount = 0; // Verify ordering
            var sut = new HostBuilder()
                .ConfigureServices((services) =>
                {
                    Assert.AreEqual(0, callCount++);
                    services.AddTransient(typeof(ServiceA), typeof(ServiceA));
                })
                .ConfigureServices((services) =>
                {
                    Assert.AreEqual(1, callCount++);
                    services.AddTransient(typeof(ServiceB), typeof(ServiceB));
                });

            using var host = sut.Build();

            Assert.AreEqual(2, callCount);

            Assert.IsNotNull(host.Services.GetRequiredService(typeof(ServiceA)));
            Assert.IsNotNull(host.Services.GetRequiredService(typeof(ServiceB)));
        }

        [TestMethod]
        public void ConfigureServices_throws_if_configureDelegate_is_null()
        {
            var sut = new HostBuilder();

            Assert.ThrowsException(typeof(ArgumentNullException), () => sut.ConfigureServices(null));
        }

        [TestMethod]
        public void Properties_are_available_in_HostBuilderContext()
        {
            const string expected0 = "value0";
            const string expected1 = "value1";

            var sut = new HostBuilder();
            
            sut.ConfigureServices((context, _) =>
            {
                Assert.AreEqual(expected0, (string)context.Properties[0]);
                Assert.AreEqual(expected1, (string)context.Properties[1]);
            });

            sut.Properties = new object[] { expected0, expected1 };

            Assert.AreEqual(expected0, (string)sut.Properties[0]);
            Assert.AreEqual(expected1, (string)sut.Properties[1]);

            using var host = sut.Build();
        }

        [TestMethod]
        public void UseDefaultServiceProvider_throws_if_configureDelegate_is_null()
        {
            var sut = new HostBuilder();

            Assert.ThrowsException(typeof(ArgumentNullException), () => sut.UseDefaultServiceProvider(null));
        }
    }
}
