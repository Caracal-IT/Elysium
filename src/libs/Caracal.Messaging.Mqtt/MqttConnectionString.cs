using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace Caracal.Messaging.Mqtt;

public sealed class MqttConnectionString
{
    public string ClientId { get; init; } = Guid.NewGuid().ToString();
    public string Host { get; init; } = "127.0.0.1";
    public int Port { get; init; } = 1883;

    public MqttProtocolVersion ProtocolVersion { get; init; } = MqttProtocolVersion.V500;

    public ManagedMqttClientOptions Build() => new ManagedMqttClientOptionsBuilder()
        .WithClientOptions(new MqttClientOptionsBuilder()
            .WithProtocolVersion(ProtocolVersion)
            .WithClientId(ClientId)
            .WithTcpServer(Host, Port)
            .Build())
        .Build();
}