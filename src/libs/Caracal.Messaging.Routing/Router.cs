using Caracal.Lang;

namespace Caracal.Messaging.Routing;

public sealed class Router : IRouter
{
    private readonly IRoutingFactory _routingFactory;
    
    public Router(IRoutingFactory routingFactory) => 
        _routingFactory = routingFactory;

    public void Dispose() { }

    public async Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        foreach (var processor in _routingFactory.GetProcessors())
            await processor.ProcessAsync(message, cancellationToken);

        return new Result<bool>(true);
    }
}