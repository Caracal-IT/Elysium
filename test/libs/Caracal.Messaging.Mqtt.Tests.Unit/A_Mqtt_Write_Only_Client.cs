// ReSharper disable InconsistentNaming

using Caracal.Text;
using MQTTnet.Extensions.ManagedClient;
using NSubstitute.ExceptionExtensions;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

[Trait("Category", "Unit")]
public sealed class A_Mqtt_Write_Only_Client
{
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token;
    private readonly IManagedMqttClient _client;
    private readonly Message _message;
    private readonly Topic _responseTopic = new() { Path = "test/responseTopic" };

    private readonly MqttWriteOnlyClient _sut;
    private readonly Topic _topic = new() { Path = "path/test" };

    public A_Mqtt_Write_Only_Client()
    {
        _client = Substitute.For<IManagedMqttClient>();
        var connection = new MqttConnection(_client, new MqttConnectionString());
        _message = new Message
        {
            Payload = "Test".GetBytes(),
            Topic = _topic,
            ResponseTopic = _responseTopic
        };

        _sut = new MqttWriteOnlyClient(connection);

        _client.IsStarted.Returns(true);
    }

    [Fact]
    public async Task Should_Return_Faulted_When_Connection_Fails()
    {
        _client.IsStarted.Returns(false);
        _client.StartAsync(Arg.Any<ManagedMqttClientOptions>()).ThrowsForAnyArgs(new Exception("Connection failed"));

        var result = await _sut.PublishAsync(_message, _cancellationToken);

        result.IsFaulted.Should().BeTrue();
        result.Exception.Should().NotBeNull();
        result.Exception!.Message.Should().Be("Connection failed");
    }

    [Fact]
    public async Task Should_Return_Successful_Publish_Message()
    {
        var result = await _sut.PublishAsync(_message, _cancellationToken);

        result.Value.Should().Be(true);
        await _client.Received(1)
            .EnqueueAsync(Arg.Is<ManagedMqttApplicationMessage>(a => IsValidEnqueueAsyncArgs(a)));
    }

    [Fact]
    public void Should_Dispose_Connection()
    {
        _sut.Dispose();

        _client.Received(1).Dispose();
    }

    private bool IsValidEnqueueAsyncArgs(ManagedMqttApplicationMessage args)
    {
        var msg = args.ApplicationMessage;

        return msg.Topic == _topic.Path
               && msg.PayloadSegment == _message.Payload
               && msg.ResponseTopic == _responseTopic.Path;
    }
}