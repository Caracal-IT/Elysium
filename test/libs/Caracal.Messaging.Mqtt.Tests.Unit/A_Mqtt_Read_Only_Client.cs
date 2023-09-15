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
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token;

    private readonly MqttReadOnlyClient _sut;
    
    public A_Mqtt_Read_Only_Client()
    {
        _client = Substitute.For<ManagedMqttClientWrapper>(Substitute.For<IManagedMqttClient>());
        var connection = new MqttConnection(_client, new MqttConnectionString());
        _sut = new MqttReadOnlyClient(connection);
        
        _client.IsStarted.Returns(true);
    }
    
    [Fact]
    public async Task Should_Return_Faulted_When_Connection_Fails()
    {
        _client.IsStarted.Returns(false);
        _client.StartAsync(Arg.Any<ManagedMqttClientOptions>()).ThrowsForAnyArgs(new Exception("Connection failed"));
        
        var result = await _sut.SubscribeAsync(_topic, _cancellationToken);
        
        result.IsFaulted.Should().BeTrue();
        result.Exception.Should().NotBeNull();
        result.Exception!.Message.Should().Be("Connection failed");
    }

    [Fact]
    public async Task Should_Return_Subscription_When_Subscribing()
    {
        var result = await _sut.SubscribeAsync(_topic, _cancellationToken);
        
        result.IsFaulted.Should().BeFalse();
        result.Exception.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeAssignableTo<MqttSubscription>();
    }
    
    [Fact]
    public async Task Should_Iterate_When_Subscribing()
    {
        var received = new StringBuilder();
        var result = await _sut.SubscribeAsync(_topic, _cancellationToken);
        
        await _client.SendApplicationMessageAsync("MockClient", "path/test", "Response 1".GetBytes());

        await Task.WhenAll(SendMessageAsync(), IterateAsync(received));
        
        result.Value.Should().BeAssignableTo<MqttSubscription>();
        received.ToString().Should().Be("Response 1Response 2");
        return;

        async Task IterateAsync(StringBuilder output)
        {
            await foreach (var item in result.Value!.GetNextAsync(TimeSpan.FromSeconds(10)).WithCancellation(_cancellationToken).ConfigureAwait(false)) 
                output.Append(item.Value!.Payload.GetString());
        }

        async Task SendMessageAsync()
        {
            await Task.Delay(100, _cancellationToken);
            await _client.SendApplicationMessageAsync("MockClient", "path/test", "Response 2".GetBytes());
            
            ((MqttSubscription) result.Value!).Channel.Writer.Complete();
        }
    }

    [Fact]
    public async Task Should_Remove_Subscription_When_Unsubscribing()
    {
        var subscriptionResult = await _sut.SubscribeAsync(_topic, _cancellationToken);

        await _client.SendApplicationMessageAsync("MockClient", "path/test", "Response 1".GetBytes());
        
        await _client.SendApplicationMessageAsync("MockClient", "path/test2", "Response 21".GetBytes());
        
        var subscription = subscriptionResult.Value!;
        var resultsBeforeUnsubscribe = await GetPublishedMessages(subscription, _cancellationToken);
        
        await subscription.UnsubscribeAsync();
         
        await _client.SendApplicationMessageAsync("MockClient", "path/test", "Response 1".GetBytes());

        var resultsAfterUnsubscribe = await GetPublishedMessages(subscription, _cancellationToken);
        
        resultsBeforeUnsubscribe.Should().Be("Response 1");
        resultsAfterUnsubscribe.Should().BeEmpty();
        ((MqttSubscription)subscription).LastException.Should().BeAssignableTo<OperationCanceledException>();
    }
    
    [Fact]
    public void Should_Dispose_Connection()
    {
        _sut.Dispose();
        _client.Received(1).Dispose();
    }
    
    [Fact]
    public async Task Should_Dispose_Subscription_Should_Unsubscribe()
    {
        var result = await _sut.SubscribeAsync(_topic, _cancellationToken);
        result.Value!.Dispose();

        _client.Events.Should().BeEmpty();
        await _client.Received(1).UnsubscribeAsync(Arg.Any<string[]>());
    }

    private static async Task<string> GetPublishedMessages(ISubscription subscription, CancellationToken cancellationToken)
    {
        var received = new StringBuilder();

        await foreach (var item in subscription.GetNextAsync(TimeSpan.FromMilliseconds(100)).WithCancellation(cancellationToken).ConfigureAwait(false)) 
            received.Append(item.Value!.Payload.GetString());
        
        return received.ToString();
    }
}