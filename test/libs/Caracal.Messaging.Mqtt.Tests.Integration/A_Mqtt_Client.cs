// ReSharper disable InconsistentNaming
namespace Caracal.Messaging.Mqtt.Tests.Integration;

public sealed class A_Mqtt_Client: IDisposable
{
    private readonly Message _message;
    private readonly Topic _topic = new () { Path = "test/integration/test1" };
    private readonly string _originalMessage = $"Request {Random.Shared.Next(1, 500)}";
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token;
    
    private readonly MqttClient _sut;

    public A_Mqtt_Client()
    {
        _message = new Message
        {
            Payload = _originalMessage.GetBytes(),
            Topic = _topic
        };
        
        _sut = new MqttClient( new MqttConnection());
    }
    
    [Fact(Timeout = 5000)]
    public async Task Should_Subscribe_And_Publish_To_A_Topic()
    {
        // Arrange
        var messageFromSubscription = string.Empty;
        
        //Act
        var subscriptionResult = await _sut.SubscribeAsync(_topic, _cancellationToken).ConfigureAwait(false);
        using var subscription = subscriptionResult.Value!;
        
        await _sut.PublishAsync(_message, _cancellationToken).ConfigureAwait(false);
        await foreach (var m in subscription.GetNextAsync(TimeSpan.FromSeconds(2)).ConfigureAwait(false))
        {
            messageFromSubscription = m.Value.Payload.GetString();
            break;
        }
        
        // Assert
        messageFromSubscription.Should().Be(_originalMessage);
    }

    public void Dispose() => _sut.Dispose();
}