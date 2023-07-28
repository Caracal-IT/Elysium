using Caracal.Lang;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttWriteOnlyClient: IWriteOnlyClient
{
    public Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}