using Caracal.IOT;
using Caracal.Lang;
using Caracal.Text;

namespace Caracal.Elysium.Services.Mocks;

public sealed class MockGateway : IGateway
{
    public Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var response = new Response
        {
            Payload = $"Message {Random.Shared.Next(0, 100)} from {nameof(MockGateway)}".GetBytes()
        };

        return Task.FromResult(new Result<Response>(response));
    }
}