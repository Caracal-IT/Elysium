using Caracal.Messaging.Ingress.Config;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Ingress;

public interface IIngressFactory{}

public sealed class IngressFactory : IIngressFactory
{
    private readonly IOptions<IngressOptions> _options;
    public IngressFactory(IOptions<IngressOptions> options)
    {
        _options = options;
    }
}