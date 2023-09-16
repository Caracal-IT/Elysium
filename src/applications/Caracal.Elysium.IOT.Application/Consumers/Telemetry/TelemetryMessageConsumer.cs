using Caracal.Elysium.IOT.Application.Messages;
using Caracal.Messaging;
using MassTransit;

namespace Caracal.Elysium.IOT.Application.Consumers.Telemetry;

public sealed class TelemetryMessageConsumer : IConsumer<TelemetryMessage>
{
    private readonly IWriteOnlyClient _client;

    public TelemetryMessageConsumer(IWriteOnlyClient client)
    {
        _client = client;
    }

    public async Task Consume(ConsumeContext<TelemetryMessage> context)
    {
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