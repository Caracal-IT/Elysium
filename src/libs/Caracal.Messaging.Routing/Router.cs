using Caracal.Lang;

namespace Caracal.Messaging.Routing;

public sealed class Router : IRouter
{
    private readonly IRouteingFactory _routeingFactory;
    
    public Router(IRouteingFactory routeingFactory) => 
        _routeingFactory = routeingFactory;

    public void Dispose() { }

    public async Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        foreach (var processor in _routeingFactory.GetProcessors())
            await processor.ProcessAsync(message, cancellationToken);

        return new Result<bool>(true);
    }
}