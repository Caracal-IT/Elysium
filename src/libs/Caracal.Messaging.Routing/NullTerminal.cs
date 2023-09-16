using Caracal.Lang;
using Caracal.Messaging.Routing.Config;
using Caracal.Text;

namespace Caracal.Messaging.Routing;

public sealed class NullTerminal(TerminalObjectOptions objectOptions) : ITerminal
{
    public Guid Id { get; } = objectOptions.Id;
    public string Name { get; } = objectOptions.Name;
    public bool IsEnabled { get; } = objectOptions.IsEnabled;
    public bool IsDefault { get; } = objectOptions.IsDefault;

    public Dictionary<string, string> Settings { get; } = objectOptions.Settings;

    public Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(message.Payload.GetString());

        return Task.FromResult(new Result<bool>(true));
    }
}