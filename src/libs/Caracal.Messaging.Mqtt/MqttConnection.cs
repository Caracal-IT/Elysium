using Caracal.Lang;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttConnection: IConnection, IDisposable
{
    private readonly IManagedMqttClient _client;
    private readonly MqttConnectionString _connectionString;

    public MqttConnection(IManagedMqttClient client, MqttConnectionString connectionString)
    {
        _client = client;
        _connectionString = connectionString;
    }

    public Task<Result<ConnectionDetails>> ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (!_client.IsStarted)
            _client.StartAsync(CreateManagedMqttClientOptions()).ConfigureAwait(false);

        return Task.FromResult(CreateResult());
    }

    public async Task<Result<ConnectionDetails>> DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (!_client.IsStarted) return CreateResult();
        
        var counter = 0;
        while (_client.PendingApplicationMessagesCount > 0 && counter < 10)
        {
            await Task.Delay(100, cancellationToken).ConfigureAwait(false);
            counter++;
        }
            
        await _client.StopAsync().ConfigureAwait(false);

        return CreateResult();
    }
    
    public void Dispose() => _client.Dispose();

    private ManagedMqttClientOptions CreateManagedMqttClientOptions()
    {
        return new ManagedMqttClientOptions
        {
            ClientOptions = new MqttClientOptionsBuilder()
                .WithClientId(_connectionString.ClientId)
                .WithTcpServer(_connectionString.Host, _connectionString.Port)
                .Build()
        };
    }
    
    private Result<ConnectionDetails> CreateResult()
    {
        return new Result<ConnectionDetails>(new MqttConnectionDetails
        {
            MqttClient = _client,
            IsConnected = _client.IsStarted
        });
    }
}