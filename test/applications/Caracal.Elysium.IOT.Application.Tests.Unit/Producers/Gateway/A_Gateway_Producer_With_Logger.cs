// ReSharper disable InconsistentNaming

using Caracal.Elysium.IOT.Application.Producers.Gateway;
using Caracal.Elysium.IOT.Application.Tests.Unit.TestHelpers;
using Caracal.IOT;
using Caracal.Lang;
using MassTransit;
using Microsoft.Extensions.Logging;
using Response = Caracal.IOT.Response;

namespace Caracal.Elysium.IOT.Application.Tests.Unit.Producers.Gateway;

[Trait("Category","Unit")]
public class A_Gateway_Producer_With_Logger
{
    private readonly IGateway _gateway;
    private readonly MockLogger<GatewayProducerWithLogger> _logger;
    private readonly CancellationTokenSource _cancellationTokenSource = new (TimeSpan.FromSeconds(2));
    private readonly CancellationToken _cancellationToken;
    
    private readonly GatewayProducerWithLogger _sut;

    public A_Gateway_Producer_With_Logger()
    {
        var bus = Substitute.For<IBus>();
        _gateway = Substitute.For<IGateway>();
        _logger = Substitute.For<MockLogger<GatewayProducerWithLogger>>();
        _cancellationToken = _cancellationTokenSource.Token;
        
        _sut = new GatewayProducerWithLogger(_logger, _gateway, bus, 50);
    }
    
    [Fact]
    public async Task Should_Log_Success_Information_When_Starting_With_Log_Level_Information()
    {
        _gateway.ExecuteAsync(_cancellationToken)
                 .Returns(Task.FromResult(new Result<Response>(new Response{ Payload = Array.Empty<byte>() })));
        
        _logger.IsEnabled(LogLevel.Information).Returns(true);
        await _sut.ExecuteAsync(_cancellationToken).ConfigureAwait(false);

        await _gateway.Received().ExecuteAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        _logger.Received(1).Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Starting Gateway Producer")));
        _logger.Received(1).Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Stopping Gateway Producer")));
        _logger.Received().Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Gateway Response:")));
    }
    
    [Fact]
    public async Task Should_Not_Log_Success_Information_When_Starting_With_Log_Level_Warning()
    {
        _gateway.ExecuteAsync(_cancellationToken)
            .Returns(Task.FromResult(new Result<Response>(new Response{ Payload = Array.Empty<byte>() })));
        
        _logger.IsEnabled(LogLevel.Warning).Returns(true);
        await _sut.ExecuteAsync(_cancellationToken).ConfigureAwait(false);

        await _gateway.Received().ExecuteAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        _logger.DidNotReceive().Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Starting Gateway Producer")));
        _logger.DidNotReceive().Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Gateway Producer stopped")));
        _logger.DidNotReceive().Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Gateway response:")));
    }
    
    [Theory]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.Warning)]
    [InlineData(LogLevel.Error)]
    public async Task Should_Log_Faulted_Information_When_Starting_With_Log_Level(LogLevel loglevel)
    {
        _gateway.ExecuteAsync(_cancellationToken)
            .Returns(Task.FromResult(new Result<Response>(new Exception("Mock Error"))));
        
        _logger.IsEnabled(Arg.Is<LogLevel>(level => loglevel <= level)).Returns(true);
        await _sut.ExecuteAsync(_cancellationToken).ConfigureAwait(false);

        await _gateway.Received().ExecuteAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        _logger.Received().Log(LogLevel.Error, Arg.Is<string>(s => s.Contains("Gateway Error: Mock Error")));
    }
    
    [Fact]
    public async Task Should_Not_Log_Faulted_Information_When_Starting_With_Log_Level_Critical()
    {
        _gateway.ExecuteAsync(_cancellationToken)
            .Returns(Task.FromResult(new Result<Response>(new Exception("Mock Error"))));
        
        _logger.IsEnabled(LogLevel.Critical).Returns(true);
        await _sut.ExecuteAsync(_cancellationToken).ConfigureAwait(false);

        await _gateway.Received().ExecuteAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        _logger.DidNotReceive().Log(LogLevel.Error, Arg.Is<string>(s => s.Contains("Gateway error: Mock Error")));
    }
}