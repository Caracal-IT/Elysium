using Caracal.Lang;
using Caracal.Messaging.Routing.Config;

namespace Caracal.Messaging.Routing;

public sealed class DefaultProcessor(ProcessorObjectOptions objectOptions) : IProcessor
{
    public Guid Id { get; } = objectOptions.Id;
    public string Name { get; } = objectOptions.Name;
    public bool IsEnabled { get; } = objectOptions.IsEnabled;
    public bool IsDefault { get; } = objectOptions.IsDefault;
    public IEnumerable<ITerminal> Terminals { get; set; } = Enumerable.Empty<ITerminal>();

    public async Task<Result<bool>> ProcessAsync(Message message, CancellationToken cancellationToken = default)
    {
        foreach (var terminal in Terminals)
            await terminal.PublishAsync(message, cancellationToken);
        
        return new Result<bool>(true);
    }
}