using Caracal.Elysium.IOT.Application.Messages;
using Caracal.IOT;
using Caracal.Lang;
using Caracal.Text;
using MassTransit;
using Response = Caracal.IOT.Response;

namespace Caracal.Elysium.IOT.Application.Producers.Gateway;

public class GatewayProducer(IGatewayRequest gatewayRequest, IBus bus, short delay = 3000) : IGatewayProducer
{
    public virtual async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await HandleResponse(await gatewayRequest.ExecuteAsync(cancellationToken), cancellationToken).ConfigureAwait(false);

            await Task.WhenAny(Task.Delay(delay, cancellationToken)).ConfigureAwait(false);
        }
    }

    protected virtual async Task HandleResponse(Result<Response> result, CancellationToken cancellationToken = default)
    {
        if (result.IsSuccess)
            await bus.Publish(new TelemetryMessage(result.Value!.Payload), cancellationToken).ConfigureAwait(false);
        else
            await bus.Publish(new TelemetryErrorMessage(result.Exception!.Message.GetBytes()), cancellationToken).ConfigureAwait(false);
    }
}