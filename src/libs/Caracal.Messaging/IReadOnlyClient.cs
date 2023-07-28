namespace Caracal.Messaging;

public interface IReadOnlyClient
{
    Task<Result<ISubscription>> SubscribeAsync(Topic topic, CancellationToken cancellationToken = default);
}