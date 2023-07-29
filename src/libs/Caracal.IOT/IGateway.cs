using Caracal.Lang;

namespace Caracal.IOT;

public interface IGateway
{
    Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken = default);
}