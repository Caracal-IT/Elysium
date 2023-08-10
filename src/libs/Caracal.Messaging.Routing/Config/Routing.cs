using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Routing.Config;

public sealed class RoutingOptions: IConfigureOptions<RoutingOptions>
{
    private readonly IConfigurationRoot _config;

    public RoutingOptions(){ }

    public RoutingOptions(IConfigurationRoot config) => _config = config;

    public IEnumerable<ClientOptions> Clients { get; init; } = Enumerable.Empty<ClientOptions>();
    public IEnumerable<TerminalOptions> Terminals { get; init; } = Enumerable.Empty<TerminalOptions>();
    
    public void Configure(RoutingOptions options)
    {
        _config.GetSection("Routing").Bind(options);
    }
}

public sealed class ClientOptions
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public bool IsEnabled { get; init; }
    public bool IsDefault { get; init; }
    public IEnumerable<string> Terminals { get; init; } = Enumerable.Empty<string>(); 
} 

public sealed class TerminalOptions
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public bool IsEnabled { get; init; }
    public bool IsDefault { get; init; }
    public Dictionary<string, string> Settings { get; init; } = new();
}