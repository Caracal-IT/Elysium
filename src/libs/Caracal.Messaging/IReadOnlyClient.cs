namespace Caracal.Messaging;

public interface IReadOnlyClient : IDisposable
{
    Task<Result<ISubscription>> SubscribeAsync(Topic topic, CancellationToken cancellationToken = default);
}