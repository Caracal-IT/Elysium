namespace Caracal.Messaging.Mqtt;

public sealed class MqttConnectionString
{
    public string ClientId { get; set; } = Guid.NewGuid().ToString();
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 1883;
}