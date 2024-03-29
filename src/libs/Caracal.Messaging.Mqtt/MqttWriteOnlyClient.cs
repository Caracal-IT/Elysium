using Caracal.Lang;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Protocol;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttWriteOnlyClient : IWriteOnlyClient
{
    private readonly MqttConnection _connection;
    private IDictionary<string, string>? _settings;

    public MqttWriteOnlyClient(IDictionary<string, string> settings)
    {
        _connection = new MqttConnection(new MqttConnectionString());
        _settings = settings;
    }

    public MqttWriteOnlyClient(MqttConnection connection)
    {
        _connection = connection;
    }

    public async Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        var conn = await _connection.ConnectAsync(cancellationToken).ConfigureAwait(false);

        if (conn.IsSuccess)
            return await OnSuccess(message, conn.Value!).ConfigureAwait(false);

        return conn.Exception!;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    private async Task<Result<bool>> OnSuccess(Message message, IConnectionDetails connectionDetails)
    {
        if (connectionDetails is MqttConnectionDetails mqttConnectionDetails)
            await mqttConnectionDetails.MqttClient!.EnqueueAsync(CreateMessage(message)).ConfigureAwait(false);

        return true;
    }

    private ManagedMqttApplicationMessage CreateMessage(Message message)
    {
        return new ManagedMqttApplicationMessage
        {
            Id = Guid.NewGuid(),
            ApplicationMessage = new MqttApplicationMessage
            {
                Topic = message.Topic.Path,
                PayloadSegment = message.Payload,
                QualityOfServiceLevel = (MqttQualityOfServiceLevel)message.Topic.QualityOfServiceLevel,
                Retain = message.Topic.Retain,
                ResponseTopic = _connection.ConnectionString.ProtocolVersion == MqttProtocolVersion.V500 ? message.ResponseTopic?.Path : null
            }
        };
    }
}