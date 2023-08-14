using Caracal.Lang;
using Caracal.Messaging.Routing.Config;
using Caracal.Text;

namespace Caracal.Messaging.Routing;

public sealed class NullTerminal: ITerminal
{
    public Guid Id { get; }
    public string Name { get; }
    public bool IsEnabled { get; }
    public bool IsDefault { get; }
    
    public Dictionary<string, string> Settings { get; }
    public Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(message.Payload.GetString());
        
        return Task.FromResult(new Result<bool>(true));
    }

    public NullTerminal(TerminalOptions options)
    {
        Id = options.Id;
        Name = options.Name;
        IsEnabled = options.IsEnabled;
        IsDefault = options.IsDefault;
        Settings = options.Settings;
    }
}