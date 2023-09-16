namespace Caracal.Elysium.IOT.Application.Ingress;

public interface IIngressController
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}