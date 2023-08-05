using Caracal.Elysium.IOT.Application.Producers.Gateway;

namespace Caracal.Elysium.Services.Services;

public sealed class DefaultWorkerService: BackgroundService
{
    private readonly IGatewayProducer _gatewayProducer;

    public DefaultWorkerService(IGatewayProducer gatewayProducer) => _gatewayProducer = gatewayProducer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _gatewayProducer.ExecuteAsync(stoppingToken).ConfigureAwait(false);
    }
}