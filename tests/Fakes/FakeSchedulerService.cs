using System;

namespace nanoFramework.Hosting.UnitTests.Fakes
{
    public class FakeSchedulerService : SchedulerService
    {
        public bool IsStarted { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public bool IsStopped { get; set; } = false;

        public FakeSchedulerService()
            : base(TimeSpan.FromMilliseconds(10))
        {
        }

        public override void Start()
        {
            IsStarted = true;

            base.Start();
        }

        protected override void ExecuteAsync()
        {
            IsCompleted = true;
        }

        public override void Stop()
        {
            IsStopped = true;

            base.Stop();
        }
    }
}
