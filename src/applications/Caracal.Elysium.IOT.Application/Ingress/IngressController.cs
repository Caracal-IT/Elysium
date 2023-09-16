using Caracal.IOT;
using Caracal.Messaging.Ingress;
using Caracal.Text;
using Microsoft.Extensions.Logging;

namespace Caracal.Elysium.IOT.Application.Ingress;

public class IngressController(IGatewayCommand gatewayCommand, IIngressFactory ingressFactory, ILogger<IngressController> logger) : IIngressController
{
    private readonly IGatewayCommand _gatewayCommand = gatewayCommand;

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var tasks = ingressFactory.GetServices()
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
            logger.LogInformation(
                "Message received: {Message}",
                msg.Value!
                    .Payload
                    .GetString()
                    .Replace("\r\n", string.Empty));
    }
}