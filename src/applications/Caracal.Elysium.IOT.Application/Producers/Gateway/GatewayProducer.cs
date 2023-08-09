using Caracal.Elysium.IOT.Application.Messages;
using Caracal.IOT;
using Caracal.Lang;
using Caracal.Text;
using MassTransit;
using Response = Caracal.IOT.Response;

namespace Caracal.Elysium.IOT.Application.Producers.Gateway;

public class GatewayProducer: IGatewayProducer
{
    private readonly short _delay;
    private readonly IGateway _gateway;
    private readonly IBus _bus;
    
    public GatewayProducer(IGateway gateway, IBus bus, short delay = 3000)
    {
        _gateway = gateway;
        _bus = bus;
        _delay = delay;
    }

    public virtual async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await HandleResponse(await _gateway.ExecuteAsync(cancellationToken), cancellationToken).ConfigureAwait(false);

            await Task.WhenAny(Task.Delay(_delay, cancellationToken)).ConfigureAwait(false);
        }
    }

    protected virtual async Task HandleResponse(Result<Response> result, CancellationToken cancellationToken = default)
    {
        if (result.IsSuccess)
            await _bus.Publish(new TelemetryMessage(result.Value!.Payload), cancellationToken).ConfigureAwait(false);
        else
            await _bus.Publish(new TelemetryErrorMessage(result.Exception!.Message.GetBytes()), cancellationToken).ConfigureAwait(false);
    }
}