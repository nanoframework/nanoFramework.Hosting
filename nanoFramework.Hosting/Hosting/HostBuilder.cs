//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Default implementation of <see cref="IHostBuilder"/>.
    /// </summary>
    public class HostBuilder : IHostBuilder
    {
        private bool _hostBuilt;
        private IServiceProvider _appServices;
        private HostBuilderContext _hostBuilderContext;
        private ServiceProviderOptions _providerOptions;

        private readonly ArrayList _configureServicesActions = new ArrayList();

        /// <inheritdoc />
        public object[] Properties { get; } = new object[0];

        /// <inheritdoc />
        public IHostBuilder ConfigureServices(ServiceContextDelegate configureDelegate)
        {
            if (configureDelegate == null)
            {
                throw new ArgumentNullException();
            }

            _configureServicesActions.Add(configureDelegate);

            return this;
        }

        /// <inheritdoc />
        public IHostBuilder UseDefaultServiceProvider(ProviderContextDelegate configureDelegate)
        {
            _providerOptions = new ServiceProviderOptions();
            configureDelegate(_hostBuilderContext, _providerOptions);
            return this;
        }

        /// <inheritdoc />
        public IHost Build()
        {
            if (_hostBuilt)
            {
                throw new InvalidOperationException();
            }
            _hostBuilt = true;

            // Create host builder context
            _hostBuilderContext = new HostBuilderContext(Properties);

            // Create service provider
            var services = new ServiceCollection();

            services.AddSingleton(typeof(IHost), typeof(Internal.Host));
            services.AddSingleton(typeof(IHostBuilder), _hostBuilderContext);

            foreach (ServiceContextDelegate configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(_hostBuilderContext, services);
            }

            if (_providerOptions == null)
            {
                _appServices = services.BuildServiceProvider();
            }
            else
            {
                _appServices = services.BuildServiceProvider(_providerOptions);
            }

            if (_appServices == null)
            {
                throw new InvalidOperationException();
            }

            return (Internal.Host)_appServices.GetRequiredService(typeof(IHost));
        }
    }
}
