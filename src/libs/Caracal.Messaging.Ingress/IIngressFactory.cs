namespace Caracal.Messaging.Ingress;

public interface IIngressFactory
{
    IEnumerable<IIngressService> GetServices();
}