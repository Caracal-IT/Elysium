using Caracal.Lang;

namespace Caracal.Messaging.Ingress;

public interface IIngressService
{
    Guid Id { get; }
    string Name { get; }
    bool IsEnabled { get; }
    bool IsDefault { get; }
    Dictionary<string, string> Settings { get; }
    
    public Task<Result<ISubscription>> SubscribeAsync(CancellationToken cancellationToken = default);
}