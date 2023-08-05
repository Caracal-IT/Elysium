using System.Diagnostics.CodeAnalysis;
using Caracal.Elysium.IOT.Application.Messages;
using MassTransit;

// ReSharper disable UnusedType.Global

namespace Caracal.Elysium.IOT.Application.Consumers.Telemetry;

[ExcludeFromCodeCoverage]
public sealed class TelemetryErrorMessageConsumer: IConsumer<TelemetryErrorMessage>
{
    public Task Consume(ConsumeContext<TelemetryErrorMessage> _) => Task.CompletedTask;
}