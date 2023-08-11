using Caracal.Lang;

namespace Caracal.Messaging.Routing;

public interface IProcessor
{
    Guid Id { get; }
    string Name { get; }
    bool IsEnabled { get; }
    bool IsDefault { get; }
    IEnumerable<ITerminal> Terminals { get; set; }

    Task<Result<bool>> ProcessAsync(Message message, CancellationToken cancellationToken = default);
}