using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
#pragma warning disable CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.

namespace Caracal.Elysium.IOT.Application.Tests.Unit.TestHelpers;

[ExcludeFromCodeCoverage]
public abstract class MockLogger<T> : ILogger<T>
{
    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) => 
        Log(logLevel, formatter(state, exception));

    public abstract void Log(LogLevel logLevel, string message);

    public virtual bool IsEnabled(LogLevel logLevel) => true;

    public abstract IDisposable BeginScope<TState>(TState state);
}