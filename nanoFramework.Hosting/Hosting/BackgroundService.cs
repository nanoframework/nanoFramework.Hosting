// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Base class for implementing a long running <see cref="IHostedService"/>.
    /// </summary>
    public abstract class BackgroundService : IHostedService, IDisposable
    {
        private Thread _executeThread;
        private CancellationTokenSource _stoppingCts;

        /// <summary>
        /// Gets the <see cref="Thread"/> that executes the background operation.
        /// </summary>
        /// <remarks>
        /// Will return <see langword="null"/> if the background operation hasn't started.
        /// </remarks>
        public virtual Thread ExecuteThread() => _executeThread;

        /// <summary>
        /// This method is called when the <see cref="IHostedService"/> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <returns>A <see cref="Thread"/> that represents the long running operations.</returns>
        protected abstract void ExecuteAsync(CancellationToken stoppingToken);

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        public virtual void StartAsync()
        {
            _stoppingCts = new CancellationTokenSource();

            // Store the thread we're executing
            _executeThread = new Thread(() =>
            {
                ExecuteAsync(_stoppingCts.Token);
            });
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        public virtual void StopAsync()
        {
            if (_executeThread == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts!.Cancel();
            }
            finally
            {
                // Wait for thread to exit
                _executeThread.Join();
                _executeThread = null;
            }
        }

        public virtual void Dispose()
        {
            _stoppingCts?.Dispose();
        }
    }
}