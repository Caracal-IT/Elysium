namespace Caracal.Messaging;

public interface IConnection
{
    bool IsConnected { get; }
    Task<Result<ConnectionDetails>> ConnectAsync(CancellationToken cancellationToken = default);
    Task<Result<ConnectionDetails>> DisconnectAsync(CancellationToken cancellationToken = default);
}