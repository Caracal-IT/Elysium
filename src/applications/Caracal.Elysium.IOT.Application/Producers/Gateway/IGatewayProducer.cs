namespace Caracal.Elysium.IOT.Application.Producers.Gateway;

public interface IGatewayProducer
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}