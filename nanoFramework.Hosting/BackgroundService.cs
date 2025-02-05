//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using Microsoft.Extensions.Hosting.Internal;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Base class for implementing a long running <see cref="IHostedService"/>.
    /// </summary>
    public abstract class BackgroundService : IHostedService, IDisposable
    {
        private Thread? _executeThread;
        private CancellationTokenSource? _stoppingCts;

        /// <summary>
        /// Gets or sets the amount of time to wait for the <see cref="ExecuteThread"/> to terminate.
        /// </summary>
        protected TimeSpan ShutdownTimeout { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Gets the <see cref="Thread"/> that executes the background operation.
        /// </summary>
        /// <remarks>
        /// Will return <see langword="null"/> if the background operation hasn't started.
        /// </remarks>
        public virtual Thread? ExecuteThread() => _executeThread;

        /// <summary>
        /// This method is called when the <see cref="IHostedService"/> starts.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)"/> is called.</param>
        /// <remarks>See <see href="https://learn.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.</remarks>
        protected abstract void ExecuteAsync(CancellationToken stoppingToken);

        /// <inheritdoc />
        public virtual void StartAsync(CancellationToken cancellationToken)
        {
            // Create linked token to allow cancelling executing task from provided token
            //_stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            // TODO: We don't have linked tokens in nanoFramework so we'll just use our own
            _stoppingCts = new CancellationTokenSource();

            // Store the thread we're executing
            _executeThread = new Thread(() => ExecuteAsync(_stoppingCts.Token));
            _executeThread.Start();
        }

        /// <inheritdoc />
        public virtual void StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executeThread is null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts!.Cancel();

                var stopped = _executeThread.TryJoin(ShutdownTimeout);

                if (!stopped)
                {
                    _executeThread.Abort();
                }
            }
            finally
            {
                _executeThread = null;
            }
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            _stoppingCts?.Cancel();
        }
    }
}
