using System.Reflection;
using Caracal.Messaging;
using Caracal.Messaging.Mqtt;
using Caracal.Messaging.Routing;
using Caracal.Messaging.Routing.Config;
using Microsoft.Extensions.Options;

namespace Caracal.Elysium.Services;

public sealed class ClientBuilder: IClientBuilder
{
    private readonly IOptions<RoutingOptions> _options;

    public ClientBuilder(IOptions<RoutingOptions> options)
    {
        _options = options;
    }

    public IClient GetClient()
    {
        var connectionString = new MqttConnectionString();
        var connection = new MqttConnection(connectionString);
        return new MqttClient(connection);
    }
}