using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using nanoFramework.Hosting.UnitTests.Fakes;
using nanoFramework.TestFramework;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class HostBuilderTests
    {
        [TestMethod]
        public void Build_registers_default_services()
        {
            using var host = new HostBuilder().Build();

            Assert.IsNotNull(host.Services.GetService(typeof(HostBuilderContext)));
            Assert.IsNotNull(host.Services.GetService(typeof(IHost)));
            Assert.IsNotNull(host.Services.GetService(typeof(IServiceProvider)));
        }

        [TestMethod]
        public void Build_throws_exception_when_called_multiple_times()
        {
            var hostBuilder = new HostBuilder();
            using (hostBuilder.Build())
            {
                Assert.ThrowsException(typeof(InvalidOperationException), () => hostBuilder.Build());
            }
        }

        [TestMethod]
        public void ConfigureServices_executes_multiple_times()
        {
            var timesCalled = 0; // Verify ordering
            using var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    Assert.AreEqual(0, timesCalled++);
                    services.AddTransient(typeof(ServiceA));
                })
                .ConfigureServices(services =>
                {
                    Assert.AreEqual(1, timesCalled++);
                    services.AddSingleton(typeof(ServiceB));
                })
                .Build();
            
            Assert.AreEqual(2, timesCalled);

            Assert.IsNotNull(host.Services.GetService(typeof(ServiceA)));
            Assert.IsNotNull(host.Services.GetService(typeof(ServiceB)));
        }

        [TestMethod]
        public void Properties_are_available_in_builder_and_context()
        {

            var hostBuilder = new HostBuilder()
                .ConfigureServices((context, _) =>
                {
                    Assert.AreEqual("value1", context.Properties["key1"]);
                    Assert.AreEqual("value2", context.Properties["key2"]);
                });

            hostBuilder.Properties["key1"] = "value1";
            hostBuilder.Properties["key2"] = "value2";

            using var _ = hostBuilder.Build();

            Assert.AreEqual("value1", hostBuilder.Properties["key1"]);
            Assert.AreEqual("value2", hostBuilder.Properties["key2"]);
        }
    }
}
