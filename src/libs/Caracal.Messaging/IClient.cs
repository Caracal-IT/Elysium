namespace Caracal.Messaging;

public interface IClient: IReadOnlyClient, IWriteOnlyClient, IDisposable
{
    Task<Result<ISubscription>> PublishCommandAsync(Message message, Topic responseTopic, CancellationToken cancellationToken = default);
}