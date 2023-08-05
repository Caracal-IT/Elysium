// ReSharper disable InconsistentNaming

using Caracal.Text;
using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

[Trait("Category","Unit")]
public class A_Mqtt_Client
{
    private readonly IManagedMqttClient _client;
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token;

    private readonly MqttClient _sut;

    public A_Mqtt_Client()
    {
        _client = Substitute.For<IManagedMqttClient>();
        var connection = new MqttConnection(_client, new MqttConnectionString());
        _sut = new MqttClient(connection);
        
        _client.IsStarted.Returns(true);
    }
    
    [Fact]
    public async Task Should_Publish_Command_And_Subscribe_To_Response()
    {
        var responseTopic = new Topic { Path = "test/responseTopic" };
        var message = new Message
        {
            Payload = "Test".GetBytes(), 
            Topic = new Topic { Path = "path/test" },
            ResponseTopic = responseTopic
        };
        
        var result = await _sut.PublishCommandAsync(message, responseTopic, _cancellationToken);

        result.Value.Should().NotBeNull();
        result.Value!.Should().BeAssignableTo<MqttSubscription>();
    }
    
    [Fact]
    public void Should_Dispose_Connection()
    {
        _sut.Dispose();
        
        _client.Received(3).Dispose();
    }
}