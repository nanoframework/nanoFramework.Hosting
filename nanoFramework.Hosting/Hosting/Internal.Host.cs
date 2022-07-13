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
                throw new ArgumentNullException();
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

            foreach (IHostedService hostedService in _hostedServices.Reverse())
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
                finally
                {
                    _hostedServices = null;
                }
            }

            if (exceptions != null)
            {
                throw new AggregateException(string.Empty, exceptions); ;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // TODO:  This is not disposing properly.  Throwing memory errors in some cases
            //((IDisposable)Services)?.Dispose();
        }
    }
}