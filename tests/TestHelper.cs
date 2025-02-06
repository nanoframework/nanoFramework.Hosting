using System;
using System.Threading;

namespace nanoFramework.Hosting.UnitTests
{
    internal static class TestHelper
    {
        private const int SleepMilliseconds = 10;
        private const int TimeoutMilliseconds = 1_000;

        public static readonly TimeSpan SleepDelay = TimeSpan.FromMilliseconds(SleepMilliseconds);

        public static void Sleep()
        {
            Thread.Sleep(SleepDelay);
        }

        public static bool WaitForEvent(this WaitHandle waitHandle)
        {
            return waitHandle.WaitOne(TimeoutMilliseconds, false);
        }
    }
}
