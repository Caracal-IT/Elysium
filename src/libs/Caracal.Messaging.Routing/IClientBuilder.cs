namespace Caracal.Messaging.Routing;

public interface IClientBuilder
{
    IClient GetClient();
    object? GetService(string assembly, string typeName);
}