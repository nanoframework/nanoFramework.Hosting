using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Represents a function that can process a request.
    /// </summary>
    /// <param name="serviceCollection">Specifies the contract for a collection of service descriptors.</param>
    public delegate void ServiceAction(IServiceCollection serviceCollection);
}
