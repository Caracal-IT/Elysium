using Caracal.Lang;

namespace Caracal.Messaging.Routing;

public interface ITerminal
{
    Guid Id { get; }
    string Name { get; }
    bool IsEnabled { get; }
    bool IsDefault { get; }
    Dictionary<string, string> Settings { get; }
    
    Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default);
}