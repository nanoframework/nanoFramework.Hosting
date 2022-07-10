using System;
using System.Threading;

using nanoFramework.TestFramework;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class BackgroundServiceTests
    {
        [TestMethod]
        public void StartStopAndDisposeBackgroundService()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(FakeBackgroundService));
                }).Build();

            var service = (FakeBackgroundService)host.Services.GetService(typeof(IHostedService));
            Assert.NotNull(service);

            host.Start();
            Assert.True(service.IsStarted);

            Thread.Sleep(10);
            Assert.True(service.IsCompleted);

            host.Stop();
            Assert.True(service.IsStopped);

            host.Dispose();
        }

        [TestMethod]
        public void StartStopBackgroundServiceThrowsAggregateException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(ExecptionBackgroundService));
                }).Build();

                Assert.Throws(typeof(AggregateException),
                    () => host.Start());

                Assert.Throws(typeof(AggregateException),
                    () => host.Stop());
        }

    }

    public class FakeBackgroundService : BackgroundService
    {
        public bool IsStarted { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsStopped { get; set; }

        public override void Start()
        {
            IsStarted = true;

            base.Start();
        }

        protected override void ExecuteAsync() 
        {
            IsCompleted = true;

            while (!CancellationRequested)
            {
                Thread.Sleep(10);
            }
        }

        public override void Stop()
        {
            IsStopped = true;

            base.Stop();
        }
    }
    
    public class ExecptionBackgroundService : BackgroundService
    {
        public override void Start()
        {
            base.Start();

            throw new NotImplementedException();
        }

        protected override void ExecuteAsync()
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
