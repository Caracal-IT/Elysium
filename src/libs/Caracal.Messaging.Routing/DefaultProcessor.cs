using Caracal.Lang;
using Caracal.Messaging.Routing.Config;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Routing;

public sealed class DefaultProcessor: IProcessor
{
    public Guid Id { get; }
    public string Name { get; }
    public bool IsEnabled { get; }
    public bool IsDefault { get; }
    public IEnumerable<ITerminal> Terminals { get; set; } = Enumerable.Empty<ITerminal>();
    
    public DefaultProcessor(ProcessorOptions options)
    {
        Id = options.Id;
        Name = options.Name;
        IsEnabled = options.IsEnabled;
        IsDefault = options.IsDefault;
    }
    
    public async Task<Result<bool>> ProcessAsync(Message message, CancellationToken cancellationToken = default)
    {
        foreach (var terminal in Terminals)
            await terminal.PublishAsync(message, cancellationToken);
        
        return new Result<bool>(true);
    }
}