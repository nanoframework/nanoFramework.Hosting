using System;
using System.Device.Gpio;
using System.Threading;

using nanoFramework.Hosting;

using Microsoft.Extensions.Logging;

namespace Hosting
{
    internal class Led3HostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IHardwareService _hardware;

        public Led3HostedService(ILoggerFactory loggerFactory, IHardwareService hardware)
        {
            _logger = loggerFactory.CreateLogger(nameof(Led3HostedService));
            _hardware = hardware;
        }

        protected override void ExecuteAsync(CancellationToken cancellationToken)
        {
            var ledPin = 30; //LD3;

            GpioPin led = _hardware.GpioController.OpenPin(ledPin, PinMode.Output);
            led.Write(PinValue.Low);

            _logger.LogInformation($"Started blinking led 3 on pin {ledPin}.");

            while (!cancellationToken.IsCancellationRequested)
            {
                led.Write(PinValue.High);
                _logger.LogInformation("Led 3 status: on");

                Thread.Sleep(300);

                led.Write(PinValue.Low);
                _logger.LogInformation("Led 3 status: off");

                Thread.Sleep(300);
            }
        }
    }
}
