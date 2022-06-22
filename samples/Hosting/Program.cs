using nanoFramework.Hosting;
using nanoFramework.Logging.Debug;
using nanoFramework.DependencyInjection;

using Microsoft.Extensions.Logging;
using System.Threading;

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
                    services.AddSingleton(typeof(ILoggerFactory), typeof(DebugLoggerFactory));
                    services.AddSingleton(typeof(IHardwareService), typeof(HardwareService));
                    services.AddHostedService(typeof(Led1HostedService));
                    services.AddHostedService(typeof(Led2HostedService));
                    services.AddHostedService(typeof(Led3HostedService));
                    services.AddHostedService(typeof(TestBackgroundService));
                });
    }
}