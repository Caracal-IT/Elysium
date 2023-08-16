using Caracal.Lang;
using Caracal.Messaging.Routing;
using Caracal.Messaging.Routing.Config;

namespace Caracal.Messaging.Mqtt;

public class MqttTerminal: ITerminal
{
    private readonly MqttClient _client;
    
    public Guid Id { get; }
    public string Name { get; }
    public bool IsEnabled { get; }
    public bool IsDefault { get; }
    public Dictionary<string, string> Settings { get; }

    public MqttTerminal(TerminalObjectOptions objectOptions)
    {
        Id = objectOptions.Id;
        Name = objectOptions.Name;
        IsEnabled = objectOptions.IsEnabled;
        IsDefault = objectOptions.IsDefault;
        Settings = objectOptions.Settings;
        
        _client = new MqttClient(Settings);
    }
    
    public Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default) =>
        _client.PublishAsync(message, cancellationToken);
}