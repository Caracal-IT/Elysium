// ReSharper disable InconsistentNaming

using Caracal.Elysium.IOT.Application.Consumers.Telemetry;
using Caracal.Elysium.IOT.Application.Messages;
using Caracal.Messaging;
using Caracal.Text;
using MassTransit;

namespace Caracal.Elysium.IOT.Application.Tests.Unit.Consumers.Telemetry;

[Trait("Category","Unit")]
public class A_Telemetry_Message_Consumer
{
    private readonly IWriteOnlyClient _client;
    private readonly ConsumeContext<TelemetryMessage> _context;
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token;

    private readonly TelemetryMessageConsumer _sut;
    
    public A_Telemetry_Message_Consumer()
    {
        _client = Substitute.For<IWriteOnlyClient>();
        _sut = new TelemetryMessageConsumer(_client);
        _context = Substitute.For<ConsumeContext<TelemetryMessage>>();
    }
    
    [Fact]
    public async Task Should_Publish_Message()
    {
        _context.CancellationToken.Returns(_cancellationToken);
        _context.Message.Returns(new TelemetryMessage { Payload = "Test Payload".GetBytes() });
        
        await _sut.Consume(_context).ConfigureAwait(false);
        
        await _client.Received(1).PublishAsync(Arg.Is<Message>(m => IsValidMessage(m)), _cancellationToken).ConfigureAwait(false);
    }

    private bool IsValidMessage(Message message)
    {
        return message.Payload.GetString().Equals("Test Payload") &&
               message.Topic.Path.StartsWith("Device/") &&
               message.Topic.QualityOfServiceLevel == 1 &&
               message.Topic.Retain;
    }
}