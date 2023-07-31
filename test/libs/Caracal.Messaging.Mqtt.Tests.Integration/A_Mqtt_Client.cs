// ReSharper disable InconsistentNaming
namespace Caracal.Messaging.Mqtt.Tests.Integration;

[Trait("Category","Integration")]
public sealed class A_Mqtt_Client: IDisposable
{
    private readonly Message _message;
    private readonly MqttConnection _connection = new ();
    private readonly Topic _topic = new () { Path = "test/integration/test1" };
    private readonly string _originalMessage = $"Request {Random.Shared.Next(1, 500)}";
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token;
    
    private readonly MqttClient _sut;

    public A_Mqtt_Client()
    {
        _message = new Message
        {
            Payload = _originalMessage.GetBytes(),
            Topic = _topic
        };
        
        _sut = new MqttClient(_connection);
    }
    
    [Fact(Timeout = 1000)]
    public async Task Should_Subscribe_And_Publish_To_A_Topic()
    {
        // Arrange
        var messageFromSubscription = string.Empty;
        
        //Act
        var subscriptionResult = await _sut.SubscribeAsync(_topic, _cancellationToken).ConfigureAwait(false);
        await _sut.PublishAsync(_message, _cancellationToken).ConfigureAwait(false);
        using var subscription = subscriptionResult.Value!;

        await foreach (var m in subscription.GetNextAsync(TimeSpan.FromSeconds(1)).ConfigureAwait(false))
        {
            messageFromSubscription = m.Value.Payload.GetString();
            break;
        }
        
        // Assert
        messageFromSubscription.Should().Be(_originalMessage);
    }

    [Fact(Timeout = 5000)]
    public async Task Should_Execute_Command_To_A_Topic()
    {
        // Arrange
        var requestMessage = $"Request {Random.Shared.Next(1, 500)}";
        var responseTask = PublishResponseAsync();
        var commandTask = PublishCommandAsync(requestMessage);
        
        // Act
        await Task.WhenAll(responseTask, commandTask).ConfigureAwait(false);
        var response = await commandTask.ConfigureAwait(false);
        
        // Assert
        response.Should().Be($"Response {requestMessage}");
        
        async Task<string> PublishCommandAsync(string msgString)
        {
            await Task.Delay(200, _cancellationToken).ConfigureAwait(false);
            var topic = new Topic { Path = $"test/command" };
            var responseTopic = new Topic { Path = $"test/response" };

            var message = new Message { Topic = topic, Payload = msgString.GetBytes()};
            var subscriptionResult = await _sut.PublishCommandAsync(message, responseTopic, CancellationToken.None).ConfigureAwait(false);
            using var subscription = subscriptionResult.Value!;
            await foreach (var m in subscription.GetNextAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false))
            {
                return m.Value.Payload.GetString();
            }
            
            return string.Empty;
        }
        
        async Task PublishResponseAsync()
        {
            var topic = new Topic { Path = $"test/command" };
            var responseTopic = new Topic { Path = $"test/response" };

            var subscriptionResult = await _sut.SubscribeAsync(topic, _cancellationToken).ConfigureAwait(false);
            using var subscription = subscriptionResult.Value!;
            
            await foreach (var m in subscription.GetNextAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false))
            {
                var responseMsg = $"Response {m.Value.Payload.GetString()}";
                var message = new Message { Topic = responseTopic, Payload = responseMsg.GetBytes() };
                await _sut.PublishAsync(message, _cancellationToken).ConfigureAwait(false);
                break;
            }
        }
    }

    public void Dispose() => _sut.Dispose();
}