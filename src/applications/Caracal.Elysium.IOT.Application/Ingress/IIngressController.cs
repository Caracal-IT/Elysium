using Caracal.IOT;
using Caracal.Messaging.Ingress;

namespace Caracal.Elysium.IOT.Application.Ingress;

public interface IIngressController
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public class IngressController: IIngressController
{
    private readonly IGatewayCommand _gatewayCommand;
    private readonly IIngressFactory _ingressFactory;

    public IngressController(IGatewayCommand gatewayCommand, IIngressFactory ingressFactory)
    {
        _gatewayCommand = gatewayCommand;
        _ingressFactory = ingressFactory;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var services = _ingressFactory.GetServices();
        
        while (!cancellationToken.IsCancellationRequested)
        {
            
            await Task.Delay(10_000, cancellationToken).ConfigureAwait(false);
        }
    }
}