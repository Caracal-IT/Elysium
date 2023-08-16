using Caracal.Lang;

namespace Caracal.Messaging.Ingress.Config;

public sealed class IngressServiceOptions: IDynamicObjectOption
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = string.Empty;
    public string Assembly { get; init; } = string.Empty; 
    public string Type { get; init; } = string.Empty;
    public bool IsEnabled { get; init; }
    public bool IsDefault { get; init; }
    public Dictionary<string, string> Settings { get; init; } = new();
}