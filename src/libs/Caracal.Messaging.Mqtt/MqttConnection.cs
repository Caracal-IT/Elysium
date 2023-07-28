using Caracal.Lang;
using MQTTnet.Client;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttConnection: IConnection
{
    private readonly IMqttClient _client;
    private readonly MqttConnectionString _connectionString;

    public MqttConnection(IMqttClient client, MqttConnectionString connectionString)
    {
        _client = client;
        _connectionString = connectionString;
    }

    public bool IsConnected => _client.IsConnected;

    public Task<Result<ConnectionDetails>> ConnectAsync(CancellationToken cancellationToken = default)
    {
        var result = new Result<ConnectionDetails>(new MqttConnectionDetails { MqttClient = _client });

        return Task.FromResult(result);
    }

    public Task<Result<ConnectionDetails>> DisconnectAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}