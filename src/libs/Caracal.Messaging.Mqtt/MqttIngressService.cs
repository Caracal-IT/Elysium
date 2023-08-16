using Caracal.Messaging.Ingress;
using Caracal.Messaging.Ingress.Config;

namespace Caracal.Messaging.Mqtt;

public class MqttIngressService: IIngressService
{
    public Guid Id { get; }
    public string Name { get; }
    public bool IsEnabled { get; }
    public bool IsDefault { get; }
    public Dictionary<string, string> Settings { get; }

    public MqttIngressService(IngressServiceOptions objectOptions)
    {
        Id = objectOptions.Id;
        Name = objectOptions.Name;
        IsEnabled = objectOptions.IsEnabled;
        IsDefault = objectOptions.IsDefault;
        Settings = objectOptions.Settings;
    }
    
    public Task<ISubscription> SubscribeAsync(string queueName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}