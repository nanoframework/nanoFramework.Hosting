//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.DependencyInjection;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Represents a function that can process a provider.
    /// </summary>
    /// <param name="configure">The delegate that configures the <see cref="ServiceProviderOptions"/>.</param>
    public delegate void ProviderAction(ServiceProviderOptions configure);
}
