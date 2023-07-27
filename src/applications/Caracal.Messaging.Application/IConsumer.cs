namespace Caracal.Messaging.Application;

public interface IConsumer<in T>: IConsumer where T: class
{
    Task ConsumeAsync(T message, CancellationToken cancellationToken = default);
}

public interface IConsumer {}