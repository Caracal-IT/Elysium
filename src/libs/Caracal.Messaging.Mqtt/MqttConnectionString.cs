using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttConnectionString
{
    public string ClientId { get; init; } = Guid.NewGuid().ToString();
    public string Host { get; init; } = "127.0.0.1";
    public int Port { get; init; } = 1883;

    public ManagedMqttClientOptions Build() => new ManagedMqttClientOptionsBuilder()
        .WithClientOptions(new MqttClientOptionsBuilder()
            .WithClientId(ClientId)
            .WithTcpServer(Host, Port)
            .Build())
        .Build();
}