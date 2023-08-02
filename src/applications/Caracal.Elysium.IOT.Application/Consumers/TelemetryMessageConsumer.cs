using Caracal.Elysium.IOT.Application.Messages;
using Caracal.Messaging;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Caracal.Elysium.IOT.Application.Consumers;

public sealed class TelemetryMessageConsumer: IConsumer<TelemetryMessage>
{
    private readonly ILogger<TelemetryMessageConsumer> _logger;
    private readonly IWriteOnlyClient _client;

    public TelemetryMessageConsumer(ILogger<TelemetryMessageConsumer> logger, IWriteOnlyClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task Consume(ConsumeContext<TelemetryMessage> context)
    {
        if(_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Telemetry message received: {Payload}", context.Message.Payload);
        
        var message = new Message
        {
            Topic = new Topic
            {
                Path = $"Device/{Random.Shared.Next(1000, 1020)}",
                QualityOfServiceLevel = 1,
                Retain = true
            },
            Payload = context.Message.Payload
        };
        
        await _client.PublishAsync(message, context.CancellationToken).ConfigureAwait(false);
    }
}