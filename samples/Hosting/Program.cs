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
            var hostBuilder = Host.CreateDefaultBuilder()
                .UseDefaultServiceProvider(options =>
                {
                    options.ValidateOnBuild = true;
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton(typeof(ILoggerFactory), typeof(DebugLoggerFactory));
                    services.AddHostedService(typeof(TestBackgroundService));
                }).Build();
            
            hostBuilder.Start();

            Thread.Sleep(5000);
            
            hostBuilder.Stop();

            Thread.Sleep(5000);

            hostBuilder.Dispose();

            Thread.Sleep(5000);

            hostBuilder.Run();
        }
    }
}