// ReSharper disable InconsistentNaming

using Caracal.Elysium.IOT.Application.Producers.Gateway;
using Caracal.Elysium.Services.Services;

namespace Caracal.Elysium.Services.Tests.Unit.Services;

[Trait("Category","Unit")]
public class The_Default_Worker_Service
{
    private readonly IGatewayProducer _gatewayProducer;
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token;
    
    private readonly GatewayProducerWorkerService _sut;
    
    public The_Default_Worker_Service()
    {
        _gatewayProducer = Substitute.For<IGatewayProducer>();
        _sut = new GatewayProducerWorkerService(_gatewayProducer);
    }
    
    [Fact]
    public async Task Should_Execute_Gateway_Producer()
    {
        await _sut.StartAsync(_cancellationToken);
        await _sut.StopAsync(_cancellationToken);
        
        await _gatewayProducer.Received(1).ExecuteAsync(Arg.Any<CancellationToken>());
    }
}