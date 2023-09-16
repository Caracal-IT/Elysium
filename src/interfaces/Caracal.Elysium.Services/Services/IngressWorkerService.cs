using Caracal.Elysium.IOT.Application.Ingress;

namespace Caracal.Elysium.Services.Services;

public sealed class IngressWorkerService(IIngressController ingressController) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return ingressController.ExecuteAsync(stoppingToken);
    }
}