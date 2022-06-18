using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Represents a function that can process a request.
    /// </summary>
    /// <param name="context">The context for the request.</param>
    /// <param name="serviceCollection">Specifies the contract for a collection of service descriptors.</param>
    public delegate void ServiceDelegate(HostBuilderContext context, IServiceCollection serviceCollection);
}
