using nanoFramework.Hosting;
using nanoFramework.Logging.Debug;
using nanoFramework.DependencyInjection;

using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace Hosting
{
    public class Program
    {
        public static void Main()
        { 
            CreateHostBuilder().Build().Run();
        }

        private static void WriteLine()
        {
            throw new NotImplementedException();
        }

        //public static IHostBuilder CreateHostBuilder() =>
        //    Host.CreateDefaultBuilder()
        //        .ConfigureServices(services =>
        //        {
        //            services.AddSingleton(typeof(ILoggerFactory), typeof(DebugLoggerFactory));
        //            services.AddSingleton(typeof(IHardwareService), typeof(HardwareService));
        //            services.AddHostedService(typeof(Led1HostedService));
        //            services.AddHostedService(typeof(Led2HostedService));
        //            services.AddHostedService(typeof(Led3HostedService));
        //            services.AddHostedService(typeof(TestBackgroundService));
        //            services.AddHostedService(typeof(TimedHostedService));
        //        });

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(typeof(BackgroundQueue));
                    //services.AddSingleton(typeof(IHardwareService), typeof(HardwareService));
                    //services.AddSingleton(typeof(ILoggerFactory), typeof(DebugLoggerFactory));
                    services.AddHostedService(typeof(PublisherService));
                    services.AddHostedService(typeof(SubscriberService));
                    services.AddHostedService(typeof(QueueMonitorService));
                    //services.AddHostedService(typeof(Led1HostedService));
                });
    }
}