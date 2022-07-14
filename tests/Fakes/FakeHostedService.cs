//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

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
