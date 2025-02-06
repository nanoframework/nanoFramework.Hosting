using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace nanoFramework.Hosting.UnitTests.Mocks
{
    internal class LoggerMock : ILogger
    {
        public void Log(LogLevel logLevel, EventId eventId, string? state, Exception? exception, MethodInfo format)
        {
            LastLoggedException = exception;
            LastLoggedLogLevel = logLevel;
            LastLoggedMessage = state;
        }

        public Exception? LastLoggedException { get; set; }

        public LogLevel LastLoggedLogLevel { get; set; }

        public string? LastLoggedMessage { get; set; }

        public bool IsEnabled(LogLevel logLevel) => true;
    }
}
