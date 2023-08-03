// ReSharper disable InconsistentNaming

using System.Text;
using Caracal.Messaging.Mqtt.Tests.Unit.TestHelpers;
using Caracal.Text;
using MQTTnet.Extensions.ManagedClient;
using NSubstitute.ExceptionExtensions;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

[Trait("Category","Unit")]
public sealed class A_Mqtt_Read_Only_Client
{
    private readonly ManagedMqttClientWrapper _client;
    private readonly Topic _topic = new() { Path = "path/test" };
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token;

    private readonly MqttReadOnlyClient _sut;
    
    public A_Mqtt_Read_Only_Client()
    {
        _client = new ManagedMqttClientWrapper(Substitute.For<IManagedMqttClient>());
        MqttConnection connection = new(_client, new MqttConnectionString());
        _sut = new MqttReadOnlyClient(connection);
        
        _client.IsStarted.Returns(true);
    }
    
    [Fact]
    public async Task Should_Return_Faulted_When_Connection_Fails()
    {
        _client.IsStarted.Returns(false);
        _client.StartAsync(Arg.Any<ManagedMqttClientOptions>()).ThrowsForAnyArgs(new Exception("Connection failed"));
        
        var result = await _sut.SubscribeAsync(_topic, _cancellationToken).ConfigureAwait(false);
        
        result.IsFaulted.Should().BeTrue();
        result.Exception.Should().NotBeNull();
        result.Exception!.Message.Should().Be("Connection failed");
    }

    [Fact]
    public async Task Should_Return_Subscription_When_Subscribing()
    {
        var result = await _sut.SubscribeAsync(_topic, _cancellationToken).ConfigureAwait(false);
        
        result.IsFaulted.Should().BeFalse();
        result.Exception.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeAssignableTo<MqttSubscription>();
    }
    
    [Fact]
    public async Task Should_Iterate_When_Subscribing()
    {
        var result = await _sut.SubscribeAsync(_topic, _cancellationToken).ConfigureAwait(false);
        await _client.SendApplicationMessageAsync("MockClient", "path/test", "Response 1".GetBytes());
        await _client.SendApplicationMessageAsync("MockClient", "path/test", "Response 2".GetBytes());
        
        var received = new StringBuilder();
        await foreach (var item in result.Value!.GetNextAsync(TimeSpan.FromSeconds(10)).WithCancellation(_cancellationToken).ConfigureAwait(false))
            received.Append(item.Value.Payload.GetString());
        
        result.Value.Should().BeAssignableTo<MqttSubscription>();
        received.ToString().Should().Be("Response 1Response 2");
    }
}