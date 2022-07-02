//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Defines methods for objects that are managed by the host.
    /// </summary>
    public interface IHostedService
    {
        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        void StartAsync();

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        void StopAsync();
    }
}
