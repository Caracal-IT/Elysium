namespace Caracal.Messaging;

public interface IConnection: IDisposable
{
    Task<Result<ConnectionDetails>> ConnectAsync(CancellationToken cancellationToken = default);
    Task<Result<ConnectionDetails>> DisconnectAsync(CancellationToken cancellationToken = default);
}