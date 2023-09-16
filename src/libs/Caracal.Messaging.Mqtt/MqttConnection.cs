using Caracal.Lang;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttConnection : IConnection, IAsyncDisposable
{
    public MqttConnection(MqttConnectionString connectionString) : this(null, connectionString)
    {
    }

    internal MqttConnection(IManagedMqttClient? client, MqttConnectionString? connectionString)
    {
        Client = client ?? new MqttFactory().CreateManagedMqttClient();
        ConnectionString = connectionString ?? new MqttConnectionString();
    }

    internal IManagedMqttClient Client { get; }
    internal MqttConnectionString ConnectionString { get; }

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

    public async Task<Result<IConnectionDetails>> ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (!Client.IsStarted)
            return await TryStartClient().ConfigureAwait(false);

        return CreateResult();
    }

    public async Task<Result<IConnectionDetails>> DisconnectAsync(CancellationToken cancellationToken = default)
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

    public void Dispose()
    {
        Client.Dispose();
    }

    private async Task<Result<IConnectionDetails>> TryStartClient()
    {
        try
        {
            await Client.StartAsync(ConnectionString.Build()).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            return new Result<IConnectionDetails>(e);
        }

        return CreateResult();
    }

    private Result<IConnectionDetails> CreateResult()
    {
        return new Result<IConnectionDetails>(new MqttConnectionDetails
        {
            MqttClient = Client
        });
    }
}