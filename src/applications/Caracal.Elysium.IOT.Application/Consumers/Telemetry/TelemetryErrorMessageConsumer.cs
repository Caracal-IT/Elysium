using Caracal.Elysium.IOT.Application.Messages;
using MassTransit;

// ReSharper disable UnusedType.Global

namespace Caracal.Elysium.IOT.Application.Consumers.Telemetry;

public sealed class TelemetryErrorMessageConsumer: IConsumer<TelemetryErrorMessage>
{
    public Task Consume(ConsumeContext<TelemetryErrorMessage> _) => Task.CompletedTask;
}