//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Extension methods for <see cref="IServiceProvider"/>.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Retrieve an array of <see cref="IHostedService"/> that have been registered with the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns>An array of <see cref="IHostedService"/>.</returns>
        public static IHostedService[] GetHostedServices(this IServiceProvider serviceProvider)
        {
            var objects = serviceProvider.GetServices(typeof(IHostedService));
            var hostedServices = new IHostedService[objects.Length];

            for (var i = 0; i < objects.Length; i++)
            {
                hostedServices[i] = (IHostedService)objects[i];
            }

            return hostedServices;
        }
    }
}