using System.Collections.ObjectModel;
using Caracal.Messaging.Routing.Config;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Routing;

public interface ITerminal
{
    Guid Id { get; }
    string Name { get; }
    bool IsEnabled { get; }
    bool IsDefault { get; }
    Dictionary<string, string> Settings { get; }
}

public sealed class NullTerminal: ITerminal
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public bool IsEnabled { get; init; }
    public bool IsDefault { get; init; }
    
    public Dictionary<string, string> Settings { get; set; }
    
    public NullTerminal(TerminalOptions options)
    {
        Id = options.Id;
        Name = options.Name;
        IsEnabled = options.IsEnabled;
        IsDefault = options.IsDefault;
        Settings = options.Settings;
    }
}