using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttConnectionDetails: IConnectionDetails, IDisposable
{
    internal IManagedMqttClient? MqttClient;

    public void Dispose() =>
        MqttClient?.Dispose();
}