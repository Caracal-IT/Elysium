using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Ingress.Config;

public sealed class IngressOptions: IConfigureOptions<IngressOptions>
{
    private readonly IConfigurationRoot? _config;
    
    public IngressOptions(){ }

    public IngressOptions(IConfigurationRoot config) => _config = config;
    
    public Guid Id { get; init; } = Guid.Empty;
    
    public void Configure(IngressOptions options)
        => _config?.GetSection("Routing").Bind(options);
}