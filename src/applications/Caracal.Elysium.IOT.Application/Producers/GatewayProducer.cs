using System.Text;
using Caracal.Elysium.IOT.Application.Messages;
using Caracal.IOT;
using Caracal.Lang;
using MassTransit;
using Microsoft.Extensions.Logging;
using Response = Caracal.IOT.Response;

namespace Caracal.Elysium.IOT.Application.Producers;

public interface IGatewayProducer
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public class GatewayProducer: IGatewayProducer
{
    private readonly IGateway _gateway;
    private readonly IBus _bus;
    
    public GatewayProducer(IGateway gateway, IBus bus)
    {
        _gateway = gateway;
        _bus = bus;
    }

    public virtual async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await HandleResponse(await _gateway.ExecuteAsync(cancellationToken), cancellationToken);

            await Task.Delay(3000, cancellationToken);
        }
    }
    
    protected virtual async Task HandleResponse(Result<Response> result, CancellationToken cancellationToken = default)
    {
        await result.Match<Task>(async response =>
            {
                var msg = new TelemetryMessage
                {
                    Payload = Encoding.UTF8.GetString(response.Payload)
                };
                
                await  _bus.Publish(msg, cancellationToken);
            }, async error =>
            {
                var msg = new TelemetryErrorMessage
                {
                    Payload = error.Message
                };
                
                await  _bus.Publish(msg, cancellationToken);
            });
    }
}

public sealed class GatewayProducerWithLogger: GatewayProducer
{
    private readonly ILogger<GatewayProducerWithLogger> _logger;

    public GatewayProducerWithLogger(ILogger<GatewayProducerWithLogger> logger, IGateway gateway, IBus bus) 
        : base(gateway, bus)  => _logger = logger;

    public override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if(_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Starting Gateway Producer");
        
        await base.ExecuteAsync(cancellationToken);

        if(_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Gateway Producer stopped");
    }

    protected override async Task HandleResponse(Result<Response> result, CancellationToken cancellationToken = default)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            result.Match(
                response => _logger.LogInformation("Gateway response: {Response}", Encoding.UTF8.GetString(response.Payload)),
                error => _logger.LogError("Gateway error: {Error}", error.Message)
            );
        }

        await base.HandleResponse(result, cancellationToken);
    }
}