//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// A program abstraction.
    /// </summary>
    public interface IHost : IDisposable
    {
        /// <summary>
        /// Gets the services configured for the program (for example, using <see cref="IHostBuilder.ConfigureServices(ServiceContextDelegate)" />).
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Starts the <see cref="IHostedService" /> objects configured for the program.
        /// </summary>
        /// <param name="cancellationToken">Used to abort program start.</param>
        void StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Attempts to gracefully stop the program.
        /// </summary>
        /// <param name="cancellationToken">Used to indicate when stop should no longer be graceful.</param>
        void StopAsync(CancellationToken cancellationToken = default);
    }
}
