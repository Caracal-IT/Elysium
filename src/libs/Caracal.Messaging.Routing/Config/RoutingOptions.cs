using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Routing.Config;

public sealed class RoutingOptions: IConfigureOptions<RoutingOptions>
{
    private readonly IConfigurationRoot? _config;

    public RoutingOptions(){ }

    public RoutingOptions(IConfigurationRoot config) => _config = config;

    public IEnumerable<ClientOptions> Clients { get; init; } = Enumerable.Empty<ClientOptions>();
    public IEnumerable<TerminalOptions> Terminals { get; init; } = Enumerable.Empty<TerminalOptions>();
    
    public void Configure(RoutingOptions options) => _config?.GetSection("Routing").Bind(options);
}