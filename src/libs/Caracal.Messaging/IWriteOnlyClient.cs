namespace Caracal.Messaging;

public interface IWriteOnlyClient
{
    Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default);
}