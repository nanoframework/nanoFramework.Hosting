//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.TestFramework;
using nanoFramework.Hosting.UnitTests.Fakes;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class BackgroundServiceTests
    {
        [TestMethod]
        public void Start_starts_background_thread()
        {
            using var sut = new FakeBackgroundService();

            try
            {
                sut.Start();
                sut.StartedEvent.WaitOne(1000, false);

                Assert.IsTrue(sut.IsStarted);
                Assert.IsFalse(sut.IsStopped);
            }
            finally
            {
                sut.Stop();
            }
        }

        [TestMethod]
        public void Stops_stops_background_thread()
        {
            using var sut = new FakeBackgroundService();

            try
            {
                sut.Start();
                sut.StartedEvent.WaitOne(1000, false);

                Assert.IsTrue(sut.IsStarted);

                sut.Stop();
                sut.StoppedEvent.WaitOne(1000, false);

                Assert.IsTrue(sut.IsStopped);
            }
            finally
            {
                sut.Stop();
            }
        }
    }
}
