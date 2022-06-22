//using System;
//using System.Device.Gpio;
//using System.Threading;

//using nanoFramework.Hosting;

//using Microsoft.Extensions.Logging;

//namespace Hosting
//{
//    internal class Led2HostedService : BackgroundService
//    {
//        private readonly ILogger _logger;
//        private readonly IHardwareService _hardware;

//        public Led2HostedService(ILoggerFactory loggerFactory, IHardwareService hardware)
//        {
//            _logger = loggerFactory.CreateLogger(nameof(Led2HostedService));
//            _hardware = hardware;
//        }

//        protected override void ExecuteAsync()
//        {
//            var ledPin = 23;  // LD2  30=LD3

//            GpioPin led = _hardware.GpioController.OpenPin(ledPin, PinMode.Output);
//            led.Write(PinValue.Low);

//            _logger.LogInformation($"Started blinking led 2 on pin {ledPin}.");

//            while (true)
//            {
//                led.Write(PinValue.High);
//                _logger.LogInformation("Led 2 status: on");

//                Thread.Sleep(200);

//                led.Write(PinValue.Low);
//                _logger.LogInformation("Led 2 status: off");
                
//                Thread.Sleep(200);
//            }
//        }
//    }
//}
