//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting.Internal
{
    /// <summary>
    /// Default implementation of <see cref="IHost"/>.
    /// </summary>
    internal class Host : IHost, IDisposable
    {
        private object[] _hostedServices;

        /// <summary>
        /// Initializes a new instance of <see cref="Host"/>.
        /// </summary>
        public Host(IServiceProvider services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Services = services;
        }

        /// <inheritdoc />
        public IServiceProvider Services { get; }

        /// <inheritdoc />
        public void Start()
        {
            _hostedServices = Services.GetServices(typeof(IHostedService));

            ArrayList exceptions = null;
            foreach (IHostedService hostedService in _hostedServices)
            {
                try
                {
                    // TODO: Thead exceptions are not passed back to main thread. What to do?
                    hostedService.Start();

                    if (hostedService is BackgroundService backgroundService)
                    {
                        backgroundService.ExecuteThread().Start();
                    }
                }
                catch (Exception ex)
                {
                    exceptions ??= new ArrayList();
                    exceptions.Add(ex);
                }
            }

            if (exceptions != null)
            {
                throw new AggregateException(string.Empty, exceptions);
            }
        }

        /// <inheritdoc />
        public void Stop()
        {
            if (_hostedServices == null)
            {
                return;
            }
            
            ArrayList exceptions = null;
            foreach (IHostedService hostedService in _hostedServices)
            {
                try
                {
                    hostedService.Stop();
                }
                catch (Exception ex)
                {
                    exceptions ??= new ArrayList();
                    exceptions.Add(ex);
                }
            }

            if (exceptions != null)
            {
                throw new AggregateException(string.Empty, exceptions); ;
            }
        }

        public void Dispose()
        {
            _hostedServices = null;
        }
    }
}