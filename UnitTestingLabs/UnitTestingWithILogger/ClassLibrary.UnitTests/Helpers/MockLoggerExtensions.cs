using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClassLibrary.UnitTests.Helpers
{
    public static class MockLoggerExtensions
    {
        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, string failMessage = null)
        {
            loggerMock.VerifyLog(level, message, Times.Once(), failMessage);
        }
        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, Times times, string failMessage = null)
        {
            loggerMock.Verify(l => l.Log(level, It.IsAny<EventId>(), It.Is<object>(o => o.ToString() == message), null,
                    It.IsAny<Func<object, Exception, string>>()),
                times, failMessage);
        }
    }
}
