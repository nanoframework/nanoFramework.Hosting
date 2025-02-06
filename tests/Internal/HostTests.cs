﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using nanoFramework.Hosting.UnitTests.Mocks;
using nanoFramework.TestFramework;

namespace nanoFramework.Hosting.UnitTests.Internal
{
    [TestClass]
    public class HostTests
    {
        [TestMethod]
        public void StartAsync_starts_IHostedServices()
        {
            var logger = new LoggerMock();
            var hostBuilder = new HostBuilder();

            hostBuilder.ConfigureServices(services =>
            {
                services.AddHostedService(_ => new BackgroundServiceMock());
                services.AddHostedService(_ => new HostedServiceMock());
                services.AddHostedService(_ => new SchedulerServiceMock());
            });

            using var host = hostBuilder.Build();

            host.StartAsync();

            var hostedServices = host.Services.GetHostedServices();

            foreach (var hostedService in hostedServices)
            {
                if (hostedService is not IHostedServiceMock mockHostedService)
                {
                    continue;
                }

                Assert.IsTrue(mockHostedService.StartAsyncCalled.WaitForEvent());
                Assert.IsTrue(mockHostedService.ExecuteAsyncCalled.WaitForEvent());
            }
        }

        [TestMethod]
        public void StartAsync_throws_AggregateException()
        {
            var logger = new LoggerMock();
            var hostBuilder = new HostBuilder();

            hostBuilder.ConfigureServices(services =>
            {
                services.AddHostedService(_ => new BackgroundServiceMock(startThrowsException: true));
                services.AddHostedService(_ => new HostedServiceMock(startThrowsException: true));
                services.AddHostedService(_ => new SchedulerServiceMock(startThrowsException: true));
                services.AddSingleton(typeof(ILogger), logger);
            });


            Assert.ThrowsException(typeof(AggregateException), () =>
            {
                using var host = hostBuilder.Build();

                host.StartAsync();
            });

            Assert.IsNotNull(logger.LastLoggedException);
            Assert.AreEqual(LogLevel.Error, logger.LastLoggedLogLevel);
        }

        [TestMethod]
        public void StopAsync_stops_IHostedServices()
        {
            var hostBuilder = new HostBuilder();

            hostBuilder.ConfigureServices(services =>
            {
                services.AddHostedService(_ => new BackgroundServiceMock());
                services.AddHostedService(_ => new HostedServiceMock());
                services.AddHostedService(_ => new SchedulerServiceMock());
            });

            using var host = hostBuilder.Build();

            host.StartAsync();

            var hostedServices = host.Services.GetHostedServices();

            foreach (var hostedService in hostedServices)
            {
                if (hostedService is not IHostedServiceMock mockHostedService)
                {
                    continue;
                }

                Assert.IsTrue(mockHostedService.StartAsyncCalled.WaitForEvent());
                Assert.IsTrue(mockHostedService.ExecuteAsyncCalled.WaitForEvent());
            }

            host.StopAsync();

            foreach (var hostedService in hostedServices)
            {
                if (hostedService is not IHostedServiceMock mockHostedService)
                {
                    continue;
                }

                Assert.IsTrue(mockHostedService.ExecuteAsyncCompleted.WaitForEvent());
                Assert.IsTrue(mockHostedService.StopAsyncCalled.WaitForEvent());
            }
        }

        [TestMethod]
        public void StopAsync_throws_AggregateException()
        {
            var hostBuilder = new HostBuilder();

            hostBuilder.ConfigureServices(services =>
            {
                services.AddHostedService(_ => new BackgroundServiceMock(stopThrowsException: true));
                services.AddHostedService(_ => new HostedServiceMock(stopThrowsException: true));
                services.AddHostedService(_ => new SchedulerServiceMock(stopThrowsException: true));
            });


            Assert.ThrowsException(typeof(AggregateException), () =>
            {
                using var host = hostBuilder.Build();

                host.StartAsync();
                host.StopAsync();
            });
        }
    }
}
