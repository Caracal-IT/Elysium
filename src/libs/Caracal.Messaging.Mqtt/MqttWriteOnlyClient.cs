using Caracal.Lang;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttWriteOnlyClient: IWriteOnlyClient
{
    private readonly MqttConnection _connection;

    public MqttWriteOnlyClient(MqttConnection connection) => _connection = connection;

    public async Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        var conn = await _connection.ConnectAsync(cancellationToken)
                                                       .ConfigureAwait(false);

        if (conn.IsSuccess)
            return await OnSuccess(message, conn.Value!);

        return conn.Exception!;
    }

    private static async Task<Result<bool>> OnSuccess(Message message, ConnectionDetails connectionDetails)
    {
        if (connectionDetails is not MqttConnectionDetails mqttConnectionDetails) return false;
        
        await mqttConnectionDetails.MqttClient
                                   .EnqueueAsync(CreateMessage(message))
                                   .ConfigureAwait(false);
        return true;
    }

    private static ManagedMqttApplicationMessage CreateMessage(Message message)
    {
        var msg = new ManagedMqttApplicationMessage()
        {
            Id = Guid.NewGuid(),
            ApplicationMessage = new MqttApplicationMessage()
            {
                Topic = message.Topic.Name,
                PayloadSegment = message.Payload,
                QualityOfServiceLevel = (MqttQualityOfServiceLevel)message.Topic.QualityOfServiceLevel,
                Retain = message.Topic.Retain
            }
        };
        
        return msg;
    }
}