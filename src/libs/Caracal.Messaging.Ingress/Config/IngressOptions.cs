using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Ingress.Config;

public sealed class IngressOptions: IConfigureOptions<IngressOptions>
{
    private readonly IConfigurationRoot? _config;
    
    public IngressOptions(){ }

    public IngressOptions(IConfigurationRoot config) => _config = config;
    
    public IEnumerable<IngressServiceOptions> Services { get; init; } = Enumerable.Empty<IngressServiceOptions>();
    
    public void Configure(IngressOptions options)
        => _config?.GetSection("Ingress").Bind(options);
}