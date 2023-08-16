using Caracal.IOT;
using Caracal.Messaging.Ingress;
using Caracal.Text;
using Caracal.Text.Json;
using Microsoft.Extensions.Logging;

namespace Caracal.Elysium.IOT.Application.Ingress;

public interface IIngressController
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public class IngressController: IIngressController
{
    private readonly IGatewayCommand _gatewayCommand;
    private readonly IIngressFactory _ingressFactory;
    private readonly ILogger<IngressController> _logger;

    public IngressController(IGatewayCommand gatewayCommand, IIngressFactory ingressFactory, ILogger<IngressController> logger)
    {
        _gatewayCommand = gatewayCommand;
        _ingressFactory = ingressFactory;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var tasks = _ingressFactory.GetServices()
                                       .Select(service => ExecuteAsync(service, cancellationToken))
                                       .ToList();

            await Task.WhenAll(tasks);
            
            await Task.Delay(60_000, cancellationToken);
        }
    }
    
    private async Task ExecuteAsync(IIngressService service, CancellationToken cancellationToken = default)
    {
        var subscription = await service.SubscribeAsync(cancellationToken).ConfigureAwait(false);

        await foreach (var msg in subscription.Value!.GetNextAsync(TimeSpan.FromDays(10)).WithCancellation(cancellationToken).ConfigureAwait(false))
            _logger.LogInformation(
                "Message received: {Message}", 
                msg.Value!
                             .Payload
                             .GetString()
                             .Replace("\r\n", string.Empty));
    }
}