using System;

namespace nanoFramework.Hosting.UnitTests.Fakes
{
    public class ExceptionHostedService : IHostedService
    {
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
