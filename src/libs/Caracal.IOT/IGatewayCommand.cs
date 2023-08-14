using Caracal.Lang;

namespace Caracal.IOT;

public interface IGatewayCommand
{
    Task<Result<Response>> ExecuteAsync(Request request, CancellationToken cancellationToken = default);
}