using Microsoft.Extensions.Logging;

namespace CareerSearcher.Tests.Mocks;

internal class Logger<T> : ILogger<T>
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {

    }

    public void LogInformation(string msg)
    {

    }
}
