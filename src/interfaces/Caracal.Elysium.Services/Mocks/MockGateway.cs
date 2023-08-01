using System.Text;
using Caracal.IOT;
using Caracal.Lang;

namespace Caracal.Elysium.Services.Mocks;

public sealed class MockGateway : IGateway
{
    public Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var response = new Response
        {
            Payload = Encoding.UTF8.GetBytes($"Message {Random.Shared.Next(0, 100)} from {nameof(MockGateway)}")
        };

        return Task.FromResult(new Result<Response>(response));
    }
}