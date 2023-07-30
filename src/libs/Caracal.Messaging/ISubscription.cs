namespace Caracal.Messaging;

public interface ISubscription
{
    Task<Result<bool>> UnsubscribeAsync(CancellationToken cancellationToken = default);
    
    IAsyncEnumerable<Result<Message>> GetNextAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<Result<Message>> GetNextAsync(TimeSpan timeoutDuration, CancellationToken cancellationToken = default);
}