//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.TestFramework;
using nanoFramework.Hosting.UnitTests.Fakes;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class HostingTests
    {
        [TestMethod]
        public void BuildAndDispose()
        {
            using IHost host = new HostBuilder().Build();
        }

        [TestMethod]
        public void StartStopHostBuilder()
        {
            var builder = new HostBuilder();
            using (var host = builder.Build())
            {
                host.Start();
                host.Stop();
            }
        }

        [TestMethod]
        public void DefaultServicesAreAvailable()
        {
            using (var host = new HostBuilder().Build())
            {
                Assert.NotNull(host.Services.GetRequiredService(typeof(IHost)));
                Assert.NotNull(host.Services.GetRequiredService(typeof(IHostBuilder)));
                Assert.NotNull(host.Services.GetRequiredService(typeof(IServiceProvider)));
            }
        }

        [TestMethod]
        public void BuildDoesNotAllowBuildingMuiltipleTimes()
        {
            HostBuilder builder = new HostBuilder();

            using (builder.Build())
            {
                Assert.Throws(typeof(InvalidOperationException),() => builder.Build());
            }
        }

        [TestMethod]
        public void ConfigureServicesCanBeCalledMultipleTimes()
        {
            var callCount = 0; // Verify ordering
            var hostBuilder = new HostBuilder()
                .ConfigureServices((services) =>
                {
                    Assert.Equal(0, callCount++);
                    services.AddTransient(typeof(ServiceA), typeof(ServiceA));
                })
                .ConfigureServices((services) =>
                {
                    Assert.Equal(1, callCount++);
                    services.AddTransient(typeof(ServiceB), typeof(ServiceB));
                });

            using (var host = hostBuilder.Build())
            {
                Assert.Equal(2, callCount);

                Assert.NotNull(host.Services.GetRequiredService(typeof(ServiceA)));
                Assert.NotNull(host.Services.GetRequiredService(typeof(ServiceB)));
            }
        }

        [TestMethod]
        public void BuilderPropertiesAreAvailableInBuilderAndContext()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    Assert.Equal("value0", (string)hostContext.Properties[0]);
                    Assert.Equal("value1", (string)hostContext.Properties[1]);
                });

            hostBuilder.Properties = new object[2];
            hostBuilder.Properties[0] = "value0";
            hostBuilder.Properties[1] = "value1";

            Assert.Equal("value0", (string)hostBuilder.Properties[0]);
            Assert.Equal("value1", (string)hostBuilder.Properties[1]);

            using (hostBuilder.Build()) { }
        }
    }
}
