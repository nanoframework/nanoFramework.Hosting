using System.Threading;
using System.Device.Gpio;

using nanoFramework.Hosting;

namespace Hosting
{
    internal class Led1HostedService : BackgroundService
    {
        private readonly IHardwareService _hardware;

        public Led1HostedService(IHardwareService hardware)
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
