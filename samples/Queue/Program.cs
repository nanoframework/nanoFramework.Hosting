//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting;
using nanoFramework.DependencyInjection;

namespace Hosting
{
    public class Program
    {
        public static void Main()
        { 
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(typeof(BackgroundQueue));
                    services.AddHostedService(typeof(PublisherService));
                    services.AddHostedService(typeof(SubscriberService));
                    services.AddHostedService(typeof(QueueMonitorService));
                });
    }
}