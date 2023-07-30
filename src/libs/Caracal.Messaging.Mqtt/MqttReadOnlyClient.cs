using Caracal.Lang;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttReadOnlyClient : IReadOnlyClient
{
    private readonly IConnection _connection;

    public MqttReadOnlyClient(IConnection connection) => _connection = connection;

    public async Task<Result<ISubscription>> SubscribeAsync(Topic topic, CancellationToken cancellationToken = default)
    {
        var conn = await _connection.ConnectAsync(cancellationToken).ConfigureAwait(false);

        return conn.Match(
            onSuccess: CreateSubscription,
            onFaulted: ex => ex);

        Result<ISubscription> CreateSubscription(ConnectionDetails details)
        {
            if (details is not MqttConnectionDetails mqttConnDetails)
                return new Result<ISubscription>(new Exception("ConnectionDetails is not of type MqttConnectionDetails"));

            return new MqttSubscription(mqttConnDetails, topic);
        }
    }
}