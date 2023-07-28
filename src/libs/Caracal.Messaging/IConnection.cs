namespace Caracal.Messaging;

public interface IConnection
{
    Task<Result<ConnectionDetails>> ConnectAsync(CancellationToken cancellationToken = default);
    Task<Result<ConnectionDetails>> DisconnectAsync(CancellationToken cancellationToken = default);
}