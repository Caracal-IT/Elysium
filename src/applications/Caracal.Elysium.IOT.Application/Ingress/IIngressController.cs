using Caracal.IOT;

namespace Caracal.Elysium.IOT.Application.Ingress;

public interface IIngressController
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public class IngressController: IIngressController
{
    private readonly IGatewayCommand _gatewayCommand;

    public IngressController(IGatewayCommand gatewayCommand)
    {
        _gatewayCommand = gatewayCommand;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            
            await Task.Delay(10_000, cancellationToken).ConfigureAwait(false);
        }
    }
}

public interface IDataSource { }  // IReadOnlyClient