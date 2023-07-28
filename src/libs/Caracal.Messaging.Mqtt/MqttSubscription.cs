using Caracal.Lang;

namespace Caracal.Messaging.Mqtt;

public class MqttSubscription: ISubscription
{
    public Task<Result<bool>> UnsubscribeAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Result<Message>> GetNextAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}