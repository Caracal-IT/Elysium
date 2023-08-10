using Caracal.Lang;

namespace Caracal.Messaging.Routing;

public sealed class Router : IRouter
{
    private readonly IClient _client;

    public Router(IClientBuilder clientBuilder)
    {
        _client = clientBuilder.GetClient();
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        return _client.PublishAsync(message, cancellationToken);
    }
}