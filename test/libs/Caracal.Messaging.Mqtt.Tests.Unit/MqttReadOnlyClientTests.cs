using Caracal.Lang;
using MQTTnet.Client;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

public class MqttReadOnlyClientTests
{
    [Fact]
    public async Task SubscriptionToTopic_ShouldReturnSubscription()
    {
        var mqttClient = Substitute.For<IMqttClient>();
        var conn = Substitute.For<IConnection>();
        var connDetails  = new MqttConnectionDetails { MqttClient = mqttClient };
        var subscription = Substitute.For<ISubscription>();

        conn.ConnectAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new Result<ConnectionDetails>(connDetails)));
        
        var sut = new MqttReadOnlyClient(conn);
        var result = await sut.SubscribeAsync(new Topic{Name = "Mock Name"}, CancellationToken.None);
        
        result.Value.Should().BeOfType<MqttSubscription>();
    }
}