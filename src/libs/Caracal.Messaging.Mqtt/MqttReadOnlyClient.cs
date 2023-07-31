using Caracal.Lang;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttReadOnlyClient : IReadOnlyClient
{
    private readonly MqttConnection _connection;

    public MqttReadOnlyClient(MqttConnection connection) => _connection = connection;

    public async Task<Result<ISubscription>> SubscribeAsync(Topic topic, CancellationToken cancellationToken = default)
    {
        var conn = await _connection.ConnectAsync(cancellationToken).ConfigureAwait(false);

        if(conn.IsFaulted)
            return new Result<ISubscription>(conn.Exception!);
        
        if(conn.Value!  is not MqttConnectionDetails mqttConnDetails)
            return new Result<ISubscription>(new Exception("ConnectionDetails is not of type MqttConnectionDetails"));

        var subscription = new MqttSubscription(mqttConnDetails, topic, cancellationToken);
        await subscription.SubscribeToTopicsAsync().ConfigureAwait(false);

        return new Result<ISubscription>(subscription);
    }

    public void Dispose() => _connection.Dispose();
}