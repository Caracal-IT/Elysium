namespace Caracal.Messaging.Routing;

public interface IRoutingFactory
{
    IEnumerable<IProcessor> GetProcessors();
}