using Caracal.Lang;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttReadOnlyClient: IReadOnlyClient
{
    private readonly IConnection _connection;

    public MqttReadOnlyClient(IConnection connection) => _connection = connection;

    public async Task<Result<ISubscription>> SubscribeAsync(Topic topic, CancellationToken cancellationToken = default)
    {
        var conn = await _connection.ConnectAsync(cancellationToken).ConfigureAwait(false);

        return conn.Match<Result<ISubscription>>(
            onSuccess: CreateSubscription,
            onFaulted: ex => ex);
    }

    private Result<ISubscription> CreateSubscription(ConnectionDetails details)
    {
        var mqttConnDetails = details as MqttConnectionDetails;
        
        return new MqttSubscription();
    }
}