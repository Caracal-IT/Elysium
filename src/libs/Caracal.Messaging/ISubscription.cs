namespace Caracal.Messaging;

public interface ISubscription: IDisposable
{
    IAsyncEnumerable<Result<Message>> GetNextAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<Result<Message>> GetNextAsync(TimeSpan timeoutDuration, CancellationToken cancellationToken = default);
}