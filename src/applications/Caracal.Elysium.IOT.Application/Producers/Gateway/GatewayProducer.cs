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
    
    
    // ReSharper disable once MemberCanBeProtected.Global
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
        await result.Match<Task>(async response =>
            {
                var msg = new TelemetryMessage { Payload = response.Payload };
                
                await  _bus.Publish(msg, cancellationToken).ConfigureAwait(false);
            }, async error =>
            {
                var msg = new TelemetryErrorMessage
                {
                    Payload = error.Message.GetBytes()
                };
                
                await  _bus.Publish(msg, cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
    }
}