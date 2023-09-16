// ReSharper disable InconsistentNaming

using Caracal.Elysium.IOT.Application.Messages;
using Caracal.Elysium.IOT.Application.Producers.Gateway;
using Caracal.IOT;
using Caracal.Lang;
using Caracal.Text;
using MassTransit;
using Response = Caracal.IOT.Response;

namespace Caracal.Elysium.IOT.Application.Tests.Unit.Producers.Gateway;

[Trait("Category", "Unit")]
public class A_Gateway_Producer
{
    private readonly IBus _bus;
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource = new(TimeSpan.FromSeconds(2));
    private readonly IGatewayRequest _gatewayRequest;

    private readonly GatewayProducer _sut;

    public A_Gateway_Producer()
    {
        _bus = Substitute.For<IBus>();
        _gatewayRequest = Substitute.For<IGatewayRequest>();
        _cancellationToken = _cancellationTokenSource.Token;

        _sut = new GatewayProducer(_gatewayRequest, _bus, 50);
    }

    [Fact]
    public async Task Should_Send_Telemetry_Message()
    {
        var results = Enumerable.Range(1, 5)
            .Select(CreateSuccessResponse)
            .ToList();

        results.Insert(2, Task.FromResult(new Result<Response>(new Exception("Test Error"))));
        _cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(210));

        _gatewayRequest.ExecuteAsync(_cancellationToken)
            .Returns(
                results[0],
                results[1..].ToArray()
            );

        _bus.Publish(Arg.Is<TelemetryMessage>(m => m.Payload.GetString().StartsWith("Test Telemetry ")), _cancellationToken)
            .Returns(Task.CompletedTask);
        _bus.Publish(Arg.Is<TelemetryErrorMessage>(m => m.Payload.GetString().Equals("Test Error")), _cancellationToken).Returns(Task.CompletedTask);

        await _sut.ExecuteAsync(_cancellationToken);

        await _bus.Received().Publish(Arg.Any<TelemetryMessage>(), _cancellationToken);
        await _bus.Received(1).Publish(Arg.Any<TelemetryErrorMessage>(), _cancellationToken);
    }

    private static Task<Result<Response>> CreateSuccessResponse(int index)
    {
        var msg = new Response { Payload = $"Test Telemetry {index}".GetBytes() };

        return Task.FromResult(new Result<Response>(msg));
    }
}