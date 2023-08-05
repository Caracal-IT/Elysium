using Caracal.IOT;
using Caracal.Lang;
using Caracal.Text;
using MassTransit;
using Microsoft.Extensions.Logging;
using Response = Caracal.IOT.Response;

namespace Caracal.Elysium.IOT.Application.Producers.Gateway;

public sealed class GatewayProducerWithLogger: GatewayProducer
{
    private readonly ILogger<GatewayProducerWithLogger> _logger;

    public GatewayProducerWithLogger(ILogger<GatewayProducerWithLogger> logger, IGateway gateway, IBus bus) 
        : base(gateway, bus)  => _logger = logger;

    public override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if(_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Starting Gateway Producer");
        
        await base.ExecuteAsync(cancellationToken).ConfigureAwait(false);

        if(_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Gateway Producer stopped");
    }

    protected override async Task HandleResponse(Result<Response> result, CancellationToken cancellationToken = default)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            result.Match(
                response => _logger.LogInformation("Gateway response: {Response}", response.Payload.GetString()),
                error => _logger.LogError("Gateway error: {Error}", error.Message)
            );
        }

        await base.HandleResponse(result, cancellationToken).ConfigureAwait(false);
    }
}