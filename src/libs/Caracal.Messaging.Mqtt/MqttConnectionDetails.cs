using MQTTnet.Client;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttConnectionDetails: ConnectionDetails
{
    public required IMqttClient MqttClient { get; init; }
}