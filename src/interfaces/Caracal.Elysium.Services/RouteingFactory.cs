using System.Reflection;
using Caracal.Messaging;
using Caracal.Messaging.Mqtt;
using Caracal.Messaging.Routing;
using Caracal.Messaging.Routing.Config;
using Microsoft.Extensions.Options;

namespace Caracal.Elysium.Services;

public sealed class RouteingFactory2: IRouteingFactory
{
    private readonly IOptions<RoutingOptions> _options;

    public RouteingFactory2(IOptions<RoutingOptions> options)
    {
        _options = options;

        var test = new RouteingFactory(options);
    }

    public IClient GetClient()
    {
        var connectionString = new MqttConnectionString();
        var connection = new MqttConnection(connectionString);
        return new MqttClient(connection);
    }
}