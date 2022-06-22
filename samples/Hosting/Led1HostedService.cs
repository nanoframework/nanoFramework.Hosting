//using System;
//using System.Device.Gpio;
//using System.Threading;

//using nanoFramework.Hosting;

//using Microsoft.Extensions.Logging;

//namespace Hosting
//{
//    internal class Led1HostedService : BackgroundService
//    {
//        private readonly ILogger _logger;
//        private readonly IHardwareService _hardware;

//        public Led1HostedService(ILoggerFactory loggerFactory, IHardwareService hardware)
//        {
//            _logger = loggerFactory.CreateLogger(nameof(Led1HostedService));
//            _hardware = hardware;
//        }

//        protected override void ExecuteAsync()
//        {
//            var ledPin = 16; //LD1;

//            GpioPin led = _hardware.GpioController.OpenPin(ledPin, PinMode.Output);
//            led.Write(PinValue.Low);

//            _logger.LogInformation($"Started blinking led 1 on pin {ledPin}.");

//            while (true)
//            {
//                led.Write(PinValue.High);
//                _logger.LogInformation("Led 1 status: on");

//                Thread.Sleep(100);

//                led.Write(PinValue.Low);
//                _logger.LogInformation("Led 1 status: off");
                
//                Thread.Sleep(100);
//            }
//        }
//    }
//}
