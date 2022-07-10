namespace nanoFramework.Hosting.UnitTests.Fakes
{
    public class FakeHostedService : IHostedService
    {
        public bool IsStarted { get; set; } = false;
        public bool IsStopped { get; set; } = false;

        public void Start()
        {
            IsStarted = true;
        }

        public void Stop()
        {
            IsStopped = true;
        }
    }
}
