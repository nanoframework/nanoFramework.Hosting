//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;
using System;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// A program initialization abstraction.
    /// </summary>
    public interface IHostBuilder
    {
        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        object[] Properties { get; set; }

        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IServiceCollection"/> that will be used
        /// to construct the <see cref="IServiceProvider"/>.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        IHostBuilder ConfigureServices(ServiceContextDelegate configureDelegate);

        /// <summary>
        /// Specify the <see cref="IServiceProvider"/> to be the default one.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="ServiceProviderOptions"/> that will be used
        /// to construct the <see cref="IServiceProvider"/>.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        IHostBuilder UseDefaultServiceProvider(ProviderContextDelegate configureDelegate);

        /// <summary>
        /// Run the given actions to initialize the host. This can only be called once.
        /// </summary>
        /// <returns>An initialized <see cref="IHost"/>.</returns>
        /// <exception cref="InvalidOperationException">"Build can only be called once."</exception>
        IHost Build();
    }
}
