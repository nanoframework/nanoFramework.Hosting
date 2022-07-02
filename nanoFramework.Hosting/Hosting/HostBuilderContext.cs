//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Context containing the common services on the <see cref="IHost" />. Some properties may be null until set by the <see cref="IHost" />.
    /// </summary>
    public class HostBuilderContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilderContext"/> class.
        /// </summary>
        public HostBuilderContext(object[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException();
            }

            Properties = properties;
        }

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public object[] Properties { get; }
    }
}