using Caracal.Lang;
using Caracal.Messaging.Ingress.Config;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Ingress;

public sealed class IngressFactory : IIngressFactory
{
    private readonly IOptions<IngressOptions> _options;
    private readonly List<IIngressService> _services = new();
    
    public IngressFactory(IOptions<IngressOptions> options)
    {
        _options = options;

        InitializeServices();
    }
    
    public IEnumerable<IIngressService> GetServices() => _services;

    private void InitializeServices()
    {
        foreach (var serviceOptions in _options.Value.Services)
        {
           var service = serviceOptions.CreateObjectFromOption<IIngressService>();
           if (service == null) continue;
           
           _services.Add(service);
        }
    }
}