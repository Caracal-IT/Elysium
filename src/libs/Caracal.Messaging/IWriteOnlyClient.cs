namespace Caracal.Messaging;

public interface IWriteOnlyClient: IDisposable
{
    Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default);
}