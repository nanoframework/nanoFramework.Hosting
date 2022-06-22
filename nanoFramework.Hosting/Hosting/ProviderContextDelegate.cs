// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.DependencyInjection;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Represents a function that can process a request.
    /// </summary>
    /// <param name="context">The context for the request.</param>
    /// <param name="configure">The delegate that configures the <see cref="IServiceProvider"/>.</param>
    public delegate void ProviderContextDelegate(HostBuilderContext context, ServiceProviderOptions configure);
}
