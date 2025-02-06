//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Extensions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionHostedServiceExtensions
    {
        /// <summary>
        /// Add an <see cref="IHostedService"/> registration for the given type.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <param name="implementationType">An <see cref="IHostedService"/> to register.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddHostedService(this IServiceCollection services, Type implementationType)
        {
            if (!implementationType.IsImplementationOf(typeof(IHostedService)))
            {
                throw new ArgumentException("Implementation type must implement IHostedService interface.");
            }

            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), implementationType));

            return services;
        }

        /// <summary>
        /// Add an <see cref="IHostedService"/> registration for the given type.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddHostedService(this IServiceCollection services, ImplementationFactoryDelegate implementationFactory)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), implementationFactory));

            return services;
        }
    }
}
