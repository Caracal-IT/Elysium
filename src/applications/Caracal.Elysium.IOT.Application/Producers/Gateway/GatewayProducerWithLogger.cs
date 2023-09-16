using Caracal.ErrorCodes;
using Caracal.IOT;
using Caracal.Lang;
using Caracal.Text;
using MassTransit;
using Microsoft.Extensions.Logging;
using Response = Caracal.IOT.Response;

namespace Caracal.Elysium.IOT.Application.Producers.Gateway;

public sealed class GatewayProducerWithLogger(ILogger<GatewayProducerWithLogger> logger, IGatewayRequest gatewayRequest, IBus bus, short delay = 3000)
    : GatewayProducer(gatewayRequest, bus, delay)
{
    public override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(GatewayCodes.GatewayStarted, "Starting Gateway Producer");

        await base.ExecuteAsync(cancellationToken).ConfigureAwait(false);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(GatewayCodes.GatewayStopped, "Stopping Gateway Producer");
    }

    protected override async Task HandleResponse(Result<Response> result, CancellationToken cancellationToken = default)
    {
        result.Match(
            response =>
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation(GatewayCodes.GatewaySuccess, "Gateway Response: {Response}", response.Payload.GetString());
            },
            error =>
            {
                if (logger.IsEnabled(LogLevel.Error))
                    logger.LogError(GatewayCodes.GatewayFailed, "Gateway Error: {Error}", error.Message);
            });

        await base.HandleResponse(result, cancellationToken).ConfigureAwait(false);
    }
}