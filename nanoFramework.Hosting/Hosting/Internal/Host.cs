using System;
using System.Collections;

using nanoFramework.DependencyInjection;
using nanoFramework.Hosting.Hosting.Internal;

using Microsoft.Extensions.Logging;

namespace nanoFramework.Hosting.Internal
{
    internal class Host : IHost, IDisposable
    {
        private object[] _hostedServices;
        private readonly ILogger _logger;

        public Host(IServiceProvider services, ILoggerFactory loggerFactory)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            Services = services;
            _logger = loggerFactory.CreateLogger(nameof(Host));
        }

        public IServiceProvider Services { get; }

        public void Start()
        {
            _logger.Starting();

            _hostedServices = Services.GetServices(typeof(IHostedService));

            ArrayList exceptions = new ArrayList();
            foreach (IHostedService hostedService in _hostedServices)
            {
                try
                {
                    // TODO: Thead exceptions are not passed back to main thread
                    hostedService.Start();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                var ex = new AggregateException("One or more hosted services failed to start.", exceptions);
                _logger.BackgroundServiceFaulted(ex);
                throw ex;
            }

            _logger.Started();
        }

        public void Stop()
        {
            _logger.Stopping();

            ArrayList exceptions = new ArrayList();
            foreach (IHostedService hostedService in _hostedServices)
            {
                try
                {
                    hostedService.Stop();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                var ex = new AggregateException("One or more hosted services failed to stop.", exceptions);
                _logger.StoppedWithException(ex);
                throw ex;
            }

            _logger.Stopped();
        }

        public void Dispose()
        {
        }
    }
}