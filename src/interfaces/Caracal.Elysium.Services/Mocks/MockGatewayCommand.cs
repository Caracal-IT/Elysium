using Caracal.IOT;
using Caracal.Lang;
using Caracal.Text;

namespace Caracal.Elysium.Services.Mocks;

public sealed class MockGatewayCommand : IGatewayCommand
{
    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken cancellationToken = default)
    {
        var response = new Response
        {
            Payload = $"Message {Random.Shared.Next(0, 100)} from {nameof(MockGatewayRequest)}".GetBytes()
        };

        await Task.Delay(100, cancellationToken).ConfigureAwait(false);

        return response;
    }
}