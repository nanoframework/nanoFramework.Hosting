using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting
{
    public static class HostingHostBuilderExtensions
    {
        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder ConfigureServices(this IHostBuilder hostBuilder, ServiceAction configureDelegate)
        {
            return hostBuilder.ConfigureServices((context, collection) => configureDelegate(collection));
        }
    }
}