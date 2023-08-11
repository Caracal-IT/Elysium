namespace Caracal.Messaging.Routing;

public interface IRouteingFactory
{
    IClient GetClient();
}