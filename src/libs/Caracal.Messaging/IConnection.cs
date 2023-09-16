// ReSharper disable UnusedParameter.Global

namespace Caracal.Messaging;

public interface IConnection : IDisposable
{
    Task<Result<IConnectionDetails>> ConnectAsync(CancellationToken cancellationToken = default);
    Task<Result<IConnectionDetails>> DisconnectAsync(CancellationToken cancellationToken = default);
}