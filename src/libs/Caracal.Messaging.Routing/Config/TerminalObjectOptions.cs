// ReSharper disable ClassNeverInstantiated.Global

using Caracal.Lang;

namespace Caracal.Messaging.Routing.Config;

public sealed class TerminalObjectOptions: IDynamicObjectOption
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = string.Empty;
    public string Assembly { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public bool IsEnabled { get; init; }
    public bool IsDefault { get; init; }
    public Dictionary<string, string> Settings { get; init; } = new();
}