// ReSharper disable InconsistentNaming

using Caracal.Messaging.Mqtt.Tests.Unit.TestHelpers;
using Caracal.Text;
using MQTTnet.Extensions.ManagedClient;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

[Trait("Category","Unit")]
public class A_Mqtt_Write_Only_Client
{
    private readonly Message _message;
    private readonly IManagedMqttClient _client;
    private readonly Topic _topic = new() { Path = "path/test" };
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token;

    private readonly MqttWriteOnlyClient _sut;

    public A_Mqtt_Write_Only_Client()
    {
        _client = Substitute.For<IManagedMqttClient>();
        var connection = new MqttConnection(_client, new MqttConnectionString());
        _message = new Message { Payload = "Test".GetBytes(), Topic = _topic };
        
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
        var b =  _client.EnqueueAsync(Arg.Any<ManagedMqttApplicationMessage>())
            .Returns(Task.CompletedTask);
        
        var result = await _sut.PublishAsync(_message, _cancellationToken);
    
        
    }
}