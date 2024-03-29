using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Routing.Config;

public sealed class RoutingOptions : IConfigureOptions<RoutingOptions>
{
    private readonly IConfigurationRoot? _config;

    public RoutingOptions()
    {
    }

    public RoutingOptions(IConfigurationRoot config)
    {
        _config = config;
    }

    public IEnumerable<ProcessorObjectOptions> Processors { get; init; } = Enumerable.Empty<ProcessorObjectOptions>();
    public IEnumerable<TerminalObjectOptions> Terminals { get; init; } = Enumerable.Empty<TerminalObjectOptions>();

    public void Configure(RoutingOptions options)
    {
        _config?.GetSection("Routing").Bind(options);
    }
}