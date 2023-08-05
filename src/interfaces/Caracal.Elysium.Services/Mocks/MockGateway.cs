using System.Diagnostics.CodeAnalysis;
using Caracal.IOT;
using Caracal.Lang;
using Caracal.Text;

namespace Caracal.Elysium.Services.Mocks;

[ExcludeFromCodeCoverage]
public sealed class MockGateway : IGateway
{
    public async Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var response = new Response
        {
            Payload = $"Message {Random.Shared.Next(0, 100)} from {nameof(MockGateway)}".GetBytes()
        };

        await Task.Delay(100, cancellationToken).ConfigureAwait(false);

        return new Result<Response>(response);
    }
}