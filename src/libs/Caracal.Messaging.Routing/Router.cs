using Caracal.Lang;

namespace Caracal.Messaging.Routing;

public sealed class Router(IRoutingFactory routingFactory) : IRouter
{
    public void Dispose() { }

    public async Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        foreach (var processor in routingFactory.GetProcessors())
            await processor.ProcessAsync(message, cancellationToken);

        return new Result<bool>(true);
    }
}