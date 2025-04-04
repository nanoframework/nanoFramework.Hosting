﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Represents a function that can process a service.
    /// </summary>
    /// <param name="serviceCollection">Specifies the contract for a collection of service descriptors.</param>
    public delegate void ServiceAction(IServiceCollection serviceCollection);
}
