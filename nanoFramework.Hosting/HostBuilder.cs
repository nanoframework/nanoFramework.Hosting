//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Default implementation of <see cref="IHostBuilder"/>.
    /// </summary>
    public class HostBuilder : IHostBuilder
    {
        private IServiceProvider? _appServices;
        private readonly ArrayList _configureServicesActions;
        private HostBuilderContext? _hostBuilderContext;
        private bool _hostBuilt;
        private readonly ServiceProviderOptions _serviceProviderOptions;

        /// <summary>
        /// Initializes a new instance of <see cref="HostBuilder"/>.
        /// </summary>
        public HostBuilder()
        {
            _configureServicesActions = new ArrayList();

            if (Debugger.IsAttached)
            {
                // Set DI validation as default when debugger is attached   
                _serviceProviderOptions = new ServiceProviderOptions
                {
                    ValidateOnBuild = true
                };
            }
            else
            {
                _serviceProviderOptions = new ServiceProviderOptions();
            }
        }

        /// <inheritdoc />
        public Hashtable Properties { get; set; } = [];

        /// <inheritdoc />
        public IHost Build()
        {
            if (_hostBuilt)
            {
                throw new InvalidOperationException();
            }

            _hostBuilt = true;

            InitializeHostBuilderContext();
            InitializeServiceProvider();

            return ResolveHost(_appServices);
        }

        /// <inheritdoc />
        public IHostBuilder ConfigureServices(ServiceContextDelegate configureDelegate)
        {
            ArgumentNullException.ThrowIfNull(configureDelegate);

            _configureServicesActions.Add(configureDelegate);
            return this;
        }

        [MemberNotNull(nameof(_hostBuilderContext))]
        private void InitializeHostBuilderContext()
        {
            _hostBuilderContext = new HostBuilderContext(Properties);
        }

        [MemberNotNull(nameof(_appServices))]
        private void InitializeServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddSingleton(typeof(IHost), typeof(Internal.Host));
            services.AddSingleton(typeof(HostBuilderContext), _hostBuilderContext);

            foreach (ServiceContextDelegate configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(_hostBuilderContext!, services);
            }

            _appServices = services.BuildServiceProvider(_serviceProviderOptions);

            if (_appServices == null)
            {
                throw new InvalidOperationException();
            }
        }

        internal static IHost ResolveHost(IServiceProvider serviceProvider)
        {
            if (serviceProvider is null)
            {
                throw new InvalidOperationException();
            }

            return (IHost)serviceProvider.GetRequiredService(typeof(IHost));
        }

        // Not sure when this call is intended to be used but if it's before build then _hostBuilderContext will be null
        /// <inheritdoc />
        public IHostBuilder UseDefaultServiceProvider(ProviderContextDelegate configureDelegate)
        {
            ArgumentNullException.ThrowIfNull(configureDelegate);

            configureDelegate(_hostBuilderContext!, _serviceProviderOptions);

            return this;
        }

    }
}
