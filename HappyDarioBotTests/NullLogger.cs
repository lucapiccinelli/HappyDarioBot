using System;
using Microsoft.Extensions.Logging;

namespace HappyDarioBotTests
{
    public class NullLogger : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public class Instance
        {
        }
    }
}