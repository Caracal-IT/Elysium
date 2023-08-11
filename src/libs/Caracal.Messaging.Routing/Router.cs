using Caracal.Lang;

namespace Caracal.Messaging.Routing;

public sealed class Router : IRouter
{
    private readonly IClient _client;
    private readonly IRouteingFactory _routeingFactory;
    
    public Router(IRouteingFactory routeingFactory)
    {
        _client = routeingFactory.GetClient();
        _routeingFactory = routeingFactory;
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        
        return _client.PublishAsync(message, cancellationToken);
    }
}