//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Context containing the common services on the <see cref="IHost" />. Some properties may be null until set by the <see cref="IHost" />.
    /// </summary>
    public class HostBuilderContext
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HostBuilderContext"/>.
        /// </summary>
        /// <param name="properties">A non-null <see cref="Hashtable"/> for sharing state between components during the host building process.</param>
        public HostBuilderContext(Hashtable properties)
        {
            ArgumentNullException.ThrowIfNull(properties);

            Properties = properties;
        }

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public Hashtable Properties { get; }
    }
}
