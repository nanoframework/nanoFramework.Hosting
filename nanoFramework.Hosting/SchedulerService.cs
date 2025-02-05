//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Base class timer service which calls an asynchronous action after the configured interval.
    /// </summary>
    public abstract class SchedulerService : IHostedService, IDisposable
    {
        private Timer? _executeTimer;
        private CancellationTokenSource? _stoppingCts;

        /// <summary>
        /// Schedules the immediate execution of <see cref="ExecuteAsync"/> on the provided interval.
        /// </summary>
        /// <param name="interval">The <see cref="TimeSpan"/> interval scheduler will execute on.</param>
        protected SchedulerService(TimeSpan interval)
        {
            Interval = interval;
            Time = TimeSpan.Zero;
        }

        /// <summary>
        /// Schedules the execution of <see cref="ExecuteAsync"/> on the provided interval.
        /// </summary>
        /// <param name="hour">The hour the scheduler will start on.</param>
        /// <param name="min">The minute the scheduler will start on.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> interval scheduler will execute on.</param>
        protected SchedulerService(int hour, int min, TimeSpan interval)
        {
            Interval = interval;

            var now = DateTime.UtcNow;
            var initialRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);

            if (now > initialRun)
            {
                initialRun = initialRun.AddDays(1);
            }

            Time = initialRun - now;

            if (Time <= TimeSpan.Zero)
            {
                Time = TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Gets the interval of the timer.
        /// </summary>
        protected TimeSpan Interval { get; }

        /// <summary>
        /// Gets the due time of the timer. 
        /// </summary>
        protected TimeSpan Time { get; }

        /// <summary>
        /// Gets the <see cref="Timer"/> that executes the background operation.
        /// </summary>
        /// <remarks>Will return <see langword="null"/> if the background operation hasn't started.</remarks>
        public virtual Timer? ExecuteTimer() => _executeTimer;

        /// <summary>
        /// This method is called each time the timer elapses. 
        /// </summary>
        protected abstract void ExecuteAsync(CancellationToken stoppingToken);

        /// <inheritdoc />
        public virtual void StartAsync(CancellationToken cancellationToken)
        {
            // Create linked token to allow cancelling executing task from provided token
            //_stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            // TODO: We don't have linked tokens in nanoFramework so we'll just use our own
            _stoppingCts = new CancellationTokenSource();

            // Store the timer we're executing
            _executeTimer = new Timer(_ =>
            {
                ExecuteAsync(_stoppingCts.Token);
            }, null, Time, Interval);
        }

        /// <inheritdoc />
        public virtual void StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executeTimer is null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts!.Cancel();
                _executeTimer.Change(Timeout.Infinite, 0);
            }
            finally
            {
                _executeTimer?.Dispose();
                _executeTimer = null;
            }
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            _stoppingCts?.Cancel();

            if (_executeTimer is not null)
            {
                _executeTimer.Change(Timeout.Infinite, 0);
                _executeTimer.Dispose();
                _executeTimer = null;
            }
        }
    }
}
