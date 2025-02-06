//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting.Internal
{
    internal sealed class Host : IHost
    {
        private IHostedService[] _hostedServices = null!;
        private readonly ILogger? _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="Host"/>.
        /// </summary>
        public Host(IServiceProvider services) : this(services, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Host"/>.
        /// </summary>
        public Host(IServiceProvider services, ILogger? logger)
        {
            ArgumentNullException.ThrowIfNull(services);

            _logger = logger;

            Services = services;
        }

        /// <inheritdoc />
        public IServiceProvider Services { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="HostBuilder"/>.</returns>
        public static HostBuilder CreateBuilder() => new();

        /// <inheritdoc />
        public void Dispose()
        {
            if (Services is IDisposable services)
            {
                services.Dispose();
            }
        }

        /// <inheritdoc />
        public void StartAsync(CancellationToken cancellationToken = default)
        {
            _hostedServices = Services.GetHostedServices();

            var exceptions = new ArrayList();

            for (var index = 0; index < _hostedServices.Length; index++)
            {
                var hostedService = _hostedServices[index];

                try
                {
                    hostedService.StartAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                var ex = new AggregateException("One or more hosted services failed to start.", exceptions);
                _logger?.LogError(ex, ex.Message);
                throw ex;
            }
        }

        /// <inheritdoc />
        public void StopAsync(CancellationToken cancellationToken = default)
        {
            var exceptions = new ArrayList();

            for (var index = _hostedServices.Length - 1; index >= 0; index--)
            {
                try
                {
                    _hostedServices[index].StopAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(string.Empty, exceptions);
            }
        }
    }
}
