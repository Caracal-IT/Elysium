using Caracal.Lang;
using Caracal.Messaging.Routing;
using Caracal.Messaging.Routing.Config;

namespace Caracal.Messaging.Mqtt;

public class MqttTerminal: ITerminal
{
    public Guid Id { get; }
    public string Name { get; }
    public bool IsEnabled { get; }
    public bool IsDefault { get; }
    public Dictionary<string, string> Settings { get; }
    
    private readonly IClient _client;

    public MqttTerminal(TerminalObjectOptions objectOptions)
    {
        Id = objectOptions.Id;
        Name = objectOptions.Name;
        IsEnabled = objectOptions.IsEnabled;
        IsDefault = objectOptions.IsDefault;
        Settings = objectOptions.Settings;
        
        var connectionString = new MqttConnectionString
        {
            Host = Settings.TryGetValue("Address", out var setting) ? setting : "127.0.0.1",
            Port = Convert.ToInt32(Settings.TryGetValue("Port", out var port) ? port : "1883")
        };
        
        var connection = new MqttConnection(connectionString);
        _client = new MqttClient(connection);
    }
    
    public Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default) =>
        _client.PublishAsync(message, cancellationToken);
}