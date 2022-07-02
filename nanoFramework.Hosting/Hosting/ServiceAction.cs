﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Represents a function that can process a service.
    /// </summary>
    /// <param name="serviceCollection">Specifies the contract for a collection of service descriptors.</param>
    public delegate void ServiceAction(IServiceCollection serviceCollection);
}
