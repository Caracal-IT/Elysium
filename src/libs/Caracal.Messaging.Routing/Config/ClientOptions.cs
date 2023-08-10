// ReSharper disable ClassNeverInstantiated.Global
namespace Caracal.Messaging.Routing.Config;

public sealed class ClientOptions
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public bool IsEnabled { get; init; }
    public bool IsDefault { get; init; }
    public IEnumerable<string> Terminals { get; init; } = Enumerable.Empty<string>(); 
}