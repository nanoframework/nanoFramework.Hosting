using System.Threading;

namespace nanoFramework.Hosting.UnitTests.Mocks
{
    internal interface IHostedServiceMock
    {
        public WaitHandle ExecuteAsyncCalled { get; }
        public WaitHandle ExecuteAsyncCompleted { get; }
        public WaitHandle StartAsyncCalled { get; }
        public WaitHandle StopAsyncCalled  { get; }
    }
}
