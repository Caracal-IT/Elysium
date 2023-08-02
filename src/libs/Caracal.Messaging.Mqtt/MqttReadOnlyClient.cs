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

        var mqttConnDetails = (MqttConnectionDetails)conn.Value!;
           
        var subscription = new MqttSubscription(mqttConnDetails, topic, cancellationToken);
        await subscription.SubscribeToTopicsAsync().ConfigureAwait(false);
        await Task.Delay(100, cancellationToken).ConfigureAwait(false);

        return new Result<ISubscription>(subscription);
    }

    public void Dispose() => _connection.Dispose();
}