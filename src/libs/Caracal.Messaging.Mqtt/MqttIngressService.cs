using Caracal.Lang;
using Caracal.Messaging.Ingress;
using Caracal.Messaging.Ingress.Config;

namespace Caracal.Messaging.Mqtt;

public class MqttIngressService: IIngressService
{
    private readonly MqttClient _client;
    
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
        
        _client = new MqttClient(Settings);
    }
    
    public async Task<Result<ISubscription>> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        var topicString = Settings.TryGetValue("Topic", out var setting) ? setting : "#";
        var topic = new Topic { Path = topicString };
        
        return await _client.SubscribeAsync(topic, cancellationToken).ConfigureAwait(false);
    }
}