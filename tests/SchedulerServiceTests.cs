using System;
using System.Threading;

using nanoFramework.TestFramework;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class SchedulerServiceTests
    {
        [TestMethod]
        public void StartStopAndDisposeSchedulerService()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(FakeSchedulerService));
                }).Build();

            var service = (FakeSchedulerService)host.Services.GetService(typeof(IHostedService));
            Assert.NotNull(service);

            host.Start();
            Assert.True(service.IsStarted);

            Thread.Sleep(20);
            Assert.True(service.IsCompleted);

            host.Stop();
            Assert.True(service.IsStopped);

            host.Dispose();
        }

        [TestMethod]
        public void StartStopSchedulerServiceThrowsAggregateException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(ExecptionSchedulerService));
                }).Build();

            Assert.Throws(typeof(AggregateException),
                () => host.Start());

            Assert.Throws(typeof(AggregateException),
                () => host.Stop());
        }
    }

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

        protected override void ExecuteAsync(object state)
        {
            IsCompleted = true;
        }

        public override void Stop()
        {
            IsStopped = true;

            base.Stop();
        }
    }

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
