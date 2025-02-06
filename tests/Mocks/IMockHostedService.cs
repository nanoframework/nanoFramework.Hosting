using System.Threading;

namespace nanoFramework.Hosting.UnitTests.Mocks
{
    internal interface IMockHostedService
    {
        public WaitHandle ExecuteAsyncCalled { get; }
        public WaitHandle ExecuteAsyncCompleted { get; }
        public WaitHandle StartAsyncCalled { get; }
        public WaitHandle StopAsyncCalled  { get; }
    }
}
