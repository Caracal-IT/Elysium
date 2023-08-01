using Caracal.Elysium.IOT.Application.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Caracal.Elysium.IOT.Application.Consumers;

public sealed class TelemetryErrorMessageConsumer: IConsumer<TelemetryErrorMessage>
{
    private readonly ILogger<TelemetryErrorMessageConsumer> _logger;

    public TelemetryErrorMessageConsumer(ILogger<TelemetryErrorMessageConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TelemetryErrorMessage> context)
    {
        if(_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Telemetry error message received: {Payload}", context.Message.Payload);
            
            
        return Task.CompletedTask;
    }
}