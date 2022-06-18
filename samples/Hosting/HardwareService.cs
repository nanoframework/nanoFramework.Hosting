using System;
using System.Threading;
using System.Device.Gpio;

using Microsoft.Extensions.Logging;

namespace Hosting
{
    internal class HardwareService : IHardwareService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly GpioController _gpioController;

        public HardwareService(ILoggerFactory loggerFactory)
        {
            _gpioController = new GpioController();
            _logger = loggerFactory.CreateLogger(nameof(HardwareService));
        }

        public GpioController GpioController { get { return _gpioController; } }

        public void Dispose()
        {
            _gpioController.Dispose();
        }
    }
}