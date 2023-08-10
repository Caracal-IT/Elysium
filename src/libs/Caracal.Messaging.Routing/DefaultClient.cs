using Caracal.Messaging.Routing.Config;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Routing;

public sealed class DefaultClient
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public bool IsEnabled { get; init; }
    public bool IsDefault { get; init; }

    public DefaultClient(IOptions<ClientOptions> options)
    {
        Id = options.Value.Id;
        Name = options.Value.Name;
        IsEnabled = options.Value.IsEnabled;
        IsDefault = options.Value.IsDefault;
    }
}