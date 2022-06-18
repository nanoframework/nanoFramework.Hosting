using nanoFramework.Hosting;
using nanoFramework.DependencyInjection;

namespace Hosting
{
    public class Program
    {
        public static void Main()
        {
            var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(typeof(IHardwareService), typeof(HardwareService));
                services.AddHostedService(typeof(Led1HostedService));
                services.AddHostedService(typeof(Led2HostedService));
                services.AddHostedService(typeof(Led3HostedService));
            }).Build();

            hostBuilder.Run();
        }
    }
}