using System.Text;
using Caracal.Lang;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

public class MqttReadOnlyClientTests
{
    [Fact]
    public async Task Test2()
    {
        await Task.Delay(0, CancellationToken.None);
        var connectionString = new MqttConnectionString();
        var mqttClient = new MqttFactory().CreateManagedMqttClient();
        var connection = new MqttConnection(mqttClient, connectionString);
        var client = new MqttWriteOnlyClient(connection);
        
        var message = new Message
        {
            Topic = new Topic
            {
                Name = "test/" + Guid.NewGuid(),
                QualityOfServiceLevel = 1,
                Retain = true
            },
            Payload = "Hello World"u8.ToArray()
        };

        var result = await client.PublishAsync(message, CancellationToken.None);

        await connection.DisconnectAsync(CancellationToken.None);
    }

    [Fact]
    public async Task SubscriptionToTopic_ShouldReturnSubscription()
    {
        var mqttClient = Substitute.For<IManagedMqttClient>();
        var conn = Substitute.For<IConnection>();
        var connDetails  = new MqttConnectionDetails { MqttClient = mqttClient };
       
        conn.ConnectAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new Result<ConnectionDetails>(connDetails)));
        
        var sut = new MqttReadOnlyClient(conn);
        var result = await sut.SubscribeAsync(new Topic{Name = "Mock Name"}, CancellationToken.None);
        
        result.Value.Should().BeOfType<MqttSubscription>();
    }
}