![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----

# Welcome to the .NET nanoFramework Hosting Library repository
Provides convenience methods for creating hosted services with preconfigured defaults.

## Build status

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoFramework.Hosting | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.Hosting/_apis/build/status/nanoframework.Hosting?repoName=nanoframework%2FnanoFramework.Hosting&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.Hosting/_build/latest?definitionId=56&repoName=nanoframework%2FnanoFramework.Hosting&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.Hosting.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Hosting/) |

## Samples

[Hosting Sample](https://github.com/nanoframework/Samples/tree/main/samples/Hosting)

## Example Hosting Container

```csharp
using System;
using System.Threading;
using System.Device.Gpio;

using nanoFramework.Hosting;
using nanoFramework.DependencyInjection;

namespace Hosting
{
    public class Program
    {
        public static void Main()
        {
            var host = CreateHostBuilder();
            host.Run();
        }

        public static IHost CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(typeof(HardwareService));
                    services.AddHostedService(typeof(LedHostedService));
                }).Build();
    }

    internal class HardwareService : IDisposable
    {
        public GpioController GpioController { get; private set; }

        public HardwareService()
        {
            GpioController = new GpioController();
        }

        public void Dispose()
        {
            GpioController.Dispose();
        }
    }

    internal class LedHostedService : BackgroundService
    {
        private readonly HardwareService _hardware;

        public LedHostedService(HardwareService hardware)
        {
            _hardware = hardware;
        }

        protected override void ExecuteAsync(CancellationToken cancellationToken)
        {
            var ledPin = 16; //LD1;

            GpioPin led = _hardware.GpioController.OpenPin(ledPin, PinMode.Output);

            while (!cancellationToken.IsCancellationRequested)
            {
                led.Toggle();
                Thread.Sleep(100);
            }
        }
    }
}
```

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## License

The **nanoFramework** Class Libraries are licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behaviour in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).