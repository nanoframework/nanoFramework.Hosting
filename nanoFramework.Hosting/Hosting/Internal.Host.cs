﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting.Internal
{
    internal class Host : IHost, IDisposable
    {
        private object[] _hostedServices;

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
                throw new AggregateException("One or more hosted services failed to start.", exceptions);
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
                throw new AggregateException("One or more hosted services failed to stop.", exceptions); ;
            }
        }

        public void Dispose()
        {
            _hostedServices = null;
        }
    }
}