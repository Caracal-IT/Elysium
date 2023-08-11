namespace Caracal.Messaging.Routing;

public interface IRouteingFactory
{
    IEnumerable<IProcessor> GetProcessors();
}