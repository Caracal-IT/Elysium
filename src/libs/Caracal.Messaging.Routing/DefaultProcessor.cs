using Caracal.Lang;
using Caracal.Messaging.Routing.Config;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Routing;

public sealed class DefaultProcessor: IProcessor
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public bool IsEnabled { get; init; }
    public bool IsDefault { get; init; }
    public IEnumerable<ITerminal> Terminals { get; set; } = Enumerable.Empty<ITerminal>();
    
    public DefaultProcessor(ProcessorOptions options)
    {
        Id = options.Id;
        Name = options.Name;
        IsEnabled = options.IsEnabled;
        IsDefault = options.IsDefault;
    }
    
    public Task<Result<bool>> ProcessAsync(Message message, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Result<bool>(true));
    }
}