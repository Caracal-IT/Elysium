using Caracal.Lang;

namespace Caracal.IOT;

public interface IGatewayRequest
{
    Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken = default);
}