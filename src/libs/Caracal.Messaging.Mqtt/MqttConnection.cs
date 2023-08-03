using Caracal.Lang;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttConnection: IConnection, IAsyncDisposable
{
    internal IManagedMqttClient Client { get; }
    internal MqttConnectionString ConnectionString { get; }

    public MqttConnection(): this(null, null) { }

    public MqttConnection(MqttConnectionString connectionString): this(null, connectionString) { }
    
    internal MqttConnection(IManagedMqttClient? client, MqttConnectionString? connectionString)
     {
        Client = client??new MqttFactory().CreateManagedMqttClient();
        ConnectionString = connectionString??new MqttConnectionString();
    }

    public async Task<Result<ConnectionDetails>> ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (!Client.IsStarted) 
            return await TryStartClient().ConfigureAwait(false);
        
        return CreateResult();
    }

    private async Task<Result<ConnectionDetails>> TryStartClient()
    {
        try
        {
            await Client.StartAsync(ConnectionString.Build()).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            return new Result<ConnectionDetails>(e);
        }

        return CreateResult();
    }

    public async Task<Result<ConnectionDetails>> DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (!Client.IsStarted) return CreateResult();
        
        var counter = 0;
        while (Client.PendingApplicationMessagesCount > 0 && counter < 10)
        {
            await Task.Delay(100, cancellationToken).ConfigureAwait(false);
            counter++;
        }
            
        await Client.StopAsync().ConfigureAwait(false);

        return CreateResult();
    }
    
    public void Dispose() =>
        Client.Dispose();

    private Result<ConnectionDetails> CreateResult()
    {
        return new Result<ConnectionDetails>(new MqttConnectionDetails
        {
            MqttClient = Client,
            IsConnected = Client.IsStarted
        });
    }

    public async ValueTask DisposeAsync()
    {
        var counter = 0;
        while (counter < 20 && Client.PendingApplicationMessagesCount > 0)
        {
            await Task.Delay(100).ConfigureAwait(false);
            counter++;
        }
        
        Dispose();
    }
}