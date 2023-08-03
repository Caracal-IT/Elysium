namespace Caracal.Messaging;

public interface ISubscription: IDisposable
{
    IAsyncEnumerable<Result<Message>> GetNextAsync(TimeSpan timeoutDuration);
    
    Task UnsubscribeAsync();
}