using System;
using System.Collections;

using nanoFramework.Logging.Debug;
using nanoFramework.DependencyInjection;

using Microsoft.Extensions.Logging;

namespace nanoFramework.Hosting
{
    public class HostBuilder : IHostBuilder
    {
        private bool _hostBuilt;
        private IServiceProvider _appServices;
        private HostBuilderContext _hostBuilderContext;
        private readonly ArrayList _configureServicesActions = new ArrayList();

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public string[] Properties { get; } = new string[0];

        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureServices(ServiceContextDelegate configureDelegate)
        {
            _configureServicesActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Run the given actions to initialize the host. This can only be called once.
        /// </summary>
        /// <returns>An initialized <see cref="IHost"/></returns>
        public IHost Build()
        {
            if (_hostBuilt)
            {
                throw new InvalidOperationException("Build can only be called once");
            }
            _hostBuilt = true;

            // Create host builder context
            _hostBuilderContext = new HostBuilderContext(Properties);

            // Create service provider
            var services = new ServiceCollection();

            services.AddSingleton(typeof(IHost), typeof(Internal.Host));
            services.AddSingleton(typeof(IHostBuilder), _hostBuilderContext);
            services.AddSingleton(typeof(ILoggerFactory), typeof(DebugLoggerFactory));

            foreach (ServiceContextDelegate configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(_hostBuilderContext, services);
            }

            _appServices = services.BuildServiceProvider();
            if (_appServices == null)
            {
                throw new InvalidOperationException($"The BuildServiceProvider returned a null ServiceProvider.");
            }

            return (Internal.Host)_appServices.GetRequiredService(typeof(IHost));
        }
    }
}
