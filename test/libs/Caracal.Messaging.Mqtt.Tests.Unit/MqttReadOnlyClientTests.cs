using System.Text;
using Caracal.Lang;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.External.RxMQTT.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

public class MqttReadOnlyClientTests
{

    [Fact]
    public async Task Test7()
    {
        using var connection = new MqttConnection();
        using var client = new MqttReadOnlyClient(connection);
        
        var topic = new Topic { Path = $"test/1" };
        var subscription = await client.SubscribeAsync(topic, CancellationToken.None);

        var b = "";
        if (subscription.Value != null)
        {
            await foreach (var m in subscription.Value.GetNextAsync(TimeSpan.FromSeconds(10), CancellationToken.None).ConfigureAwait(false))
                b += Encoding.UTF8.GetString(m.Value.Payload);
        }
    }
}