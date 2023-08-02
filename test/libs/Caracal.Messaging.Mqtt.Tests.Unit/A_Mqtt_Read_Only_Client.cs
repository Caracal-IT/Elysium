// ReSharper disable InconsistentNaming

using Caracal.Lang;
using Caracal.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using NSubstitute.ExceptionExtensions;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

[Trait("Category","Unit")]
public sealed class A_Mqtt_Read_Only_Client
{
    private readonly MqttConnection _connection;
    private readonly IManagedMqttClient _client;
    private readonly Topic _topic = new Topic { Path = "path/test" };
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token;

    private readonly MqttReadOnlyClient _sut;
    
    public A_Mqtt_Read_Only_Client()
    {
        _client = Substitute.For<IManagedMqttClient>();
        _connection = new(_client, new MqttConnectionString());;
        _sut = new MqttReadOnlyClient(_connection);
        
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
        /*
        _client.ApplicationMessageReceivedAsync +=
            Raise.Event<Func<MqttApplicationMessageReceivedEventArgs, Task>>(this,
                new MqttApplicationMessageReceivedEventArgs(
                    "clientId",
                    new MqttApplicationMessage
                    {
                        Topic = "path/test",
                        PayloadSegment = "test".GetBytes(),
                    },
                    new MqttPublishPacket(),
                    null));

        await foreach (var item in result.Value!.GetNextAsync(TimeSpan.FromMilliseconds(10000)).ConfigureAwait(false))
        {
            
        }
        
*/
        Assert.Fail("Should have iterated");
    }
}