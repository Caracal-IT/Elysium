using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttConnectionDetails: ConnectionDetails
{
    public required IManagedMqttClient MqttClient { get; init; }
}