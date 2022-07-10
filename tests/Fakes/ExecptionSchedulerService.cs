using System;

namespace nanoFramework.Hosting.UnitTests.Fakes
{
    public class ExecptionSchedulerService : SchedulerService
    {
        public ExecptionSchedulerService()
           : base(TimeSpan.FromSeconds(1))
        {
        }

        public override void Start()
        {
            base.Start();

            throw new NotImplementedException();
        }

        protected override void ExecuteAsync(object state)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            base.Stop();

            throw new NotImplementedException();
        }
    }
}
