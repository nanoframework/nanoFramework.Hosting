//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// Base class timer service which calls an asynchronous action after the configured interval.
    /// </summary>
    public abstract class SchedulerService : IHostedService, IDisposable
    {
        private TimeSpan _time;
        private Timer _executeTimer;

        /// <summary>
        /// Schedules the immediate execution of <see cref="ExecuteAsync"/> on the provided interval.
        /// </summary>
        /// <param name="interval">The <see cref="TimeSpan"/> interval scheduler will execute on.</param>
        protected SchedulerService(TimeSpan interval)
        {
            Interval = interval;
            _time = TimeSpan.Zero;
        }

        /// <summary>
        /// Schedules the execution of <see cref="ExecuteAsync"/> on the provided interval.
        /// </summary>
        /// <param name="hour">The hour the scheduler will start on.</param>
        /// <param name="min">The miniute the scheduler will start on.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> interval scheduler will execute on.</param>
        protected SchedulerService(int hour, int min, TimeSpan interval)
        {
            Interval = interval;

            DateTime now = DateTime.UtcNow;
            DateTime initialRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);

            if (now > initialRun)
            {
                initialRun = initialRun.AddDays(1);
            }

            _time = initialRun - now;

            if (_time <= TimeSpan.Zero)
            {
                _time = TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Gets or sets the interval of the timer. This can be changed even after
        /// the timer was started and will be used on the next round.
        /// </summary>
        protected TimeSpan Interval { get; set; }

        /// <summary>
        /// This method is called each time the timer elapses. 
        /// </summary>
        protected abstract void ExecuteAsync();

        /// <inheritdoc />
        public virtual void Start()
        {
            _executeTimer = new Timer(state =>
            {
                ExecuteAsync();
            }, null, _time, Interval);
        }

        /// <inheritdoc />
        public virtual void Stop()
        {
            if (_executeTimer == null)
            {
                return;
            }

            try
            {
                _executeTimer.Change(Timeout.Infinite, 0);
            }
            finally
            {
                _executeTimer = null;
            }
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            _executeTimer?.Dispose();
        }
    }
}