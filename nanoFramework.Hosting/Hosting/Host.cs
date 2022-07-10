//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="IHostBuilder"/> with pre-configured defaults.
    /// </summary>
    public static class Host
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateDefaultBuilder()
        {
            return new HostBuilder();
        }
    }
}