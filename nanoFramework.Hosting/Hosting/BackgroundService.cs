//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

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

        /// <summary>
        /// Gets or sets whether cancellation has been requested for this service.
        /// </summary>
        protected bool CancellationRequested { get; set; } = false;

        /// <summary>
        /// Gets or sets the amount of time to wait for the <see cref="Thread"/> to terminate.
        /// </summary>
        protected TimeSpan ShutdownTimeout { get; set; } = TimeSpan.FromSeconds(10);

        /// <inheritdoc />
        public virtual void Dispose() { }

        /// <summary>
        /// This method is called when the <see cref="IHostedService"/> starts.
        /// </summary>
        protected abstract void ExecuteAsync();

        /// <inheritdoc />
        public void Start()
        {
            // Store the thread we're executing
            _executeThread = new Thread(ExecuteAsync);
            _executeThread.Start();
        }

        /// <inheritdoc />
        public void Stop()
        {
            // Signal cancellation to the executing method
            CancellationRequested = true;

            if (_executeThread == null)
            {
                return;
            }

            try
            {
                // Wait for thread to exit
                _executeThread.Join(ShutdownTimeout);
            }
            finally
            {
                _executeThread = null;
            }
        }
    }
}
