//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;

namespace nanoFramework.Hosting
{

    /// <summary>
    /// Extensions for <see cref="IHost"/>.
    /// </summary>
    public static class HostingAbstractionsHostExtensions
    {
        /// <summary>
        /// Runs an application and block the calling thread.
        /// </summary>
        /// <param name="host">The <see cref="IHost"/> to run.</param>
        public static void Run(this IHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException();
            }

            host.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
