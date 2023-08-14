using Caracal.Elysium.IOT.Application.Ingress;

namespace Caracal.Elysium.Services.Services;

public sealed class IngressWorkerService: BackgroundService
{
    private readonly IIngressController _ingressController;

    public IngressWorkerService(IIngressController ingressController) => 
        _ingressController = ingressController;

    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        _ingressController.ExecuteAsync(stoppingToken);
}