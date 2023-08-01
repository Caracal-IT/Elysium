using MassTransit;

namespace Caracal.Messaging.Console.Demo;

public class PingPublisher: BackgroundService
{
    private readonly ILogger<PingPublisher> _logger;
    private readonly IBus _bus;

    public PingPublisher(ILogger<PingPublisher> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();
            
            var keyPressed = System.Console.ReadKey(true);

            if (keyPressed.Key != ConsoleKey.Escape)
            {
                _logger.LogInformation("Pressed {Button}", keyPressed.Key.ToString());
                await _bus.Publish(new Ping(keyPressed.Key.ToString()), stoppingToken).ConfigureAwait(false);
            }

            await Task.Delay(200, stoppingToken).ConfigureAwait(false);
        }
    }
}