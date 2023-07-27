namespace Caracal.Messaging.Application;

public interface IProducer {
    Task PublishAsync(CancellationToken cancellationToken = default);
}