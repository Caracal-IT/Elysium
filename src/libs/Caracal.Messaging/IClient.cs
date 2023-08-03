namespace Caracal.Messaging;

public interface IClient: IReadOnlyClient, IWriteOnlyClient
{
    Task<Result<ISubscription>> PublishCommandAsync(Message message, Topic responseTopic, CancellationToken cancellationToken = default);
}