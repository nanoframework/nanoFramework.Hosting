//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Microsoft.Extensions.DependencyInjection;

namespace nanoFramework.Hosting
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
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> or <paramref name="implementationType"/> can't be null</exception>
        /// <exception cref="ArgumentException">Implementation type must inherit <see cref="IHostedService"/> interface.</exception>
        public static IServiceCollection AddHostedService(this IServiceCollection services, Type implementationType)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException();
            }

            foreach (Type interfaceType in implementationType.GetInterfaces())
            {
                if (interfaceType.Equals(typeof(IHostedService)))
                {
                    return services.AddSingleton(typeof(IHostedService), implementationType);
                }
            }

            throw new ArgumentException();
        }
    }
}
