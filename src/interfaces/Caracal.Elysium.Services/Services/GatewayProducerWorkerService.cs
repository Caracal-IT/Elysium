using Caracal.Elysium.IOT.Application.Producers.Gateway;

namespace Caracal.Elysium.Services.Services;

public sealed class GatewayProducerWorkerService: BackgroundService
{
    private readonly IGatewayProducer _gatewayProducer;

    public GatewayProducerWorkerService(IGatewayProducer gatewayProducer) => _gatewayProducer = gatewayProducer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) => 
        await _gatewayProducer.ExecuteAsync(stoppingToken).ConfigureAwait(false);
}