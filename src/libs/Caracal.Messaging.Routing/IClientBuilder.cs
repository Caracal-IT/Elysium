namespace Caracal.Messaging.Routing;

public interface IClientBuilder
{
    IClient GetClient();
}