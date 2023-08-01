// ReSharper disable InconsistentNaming
using Caracal.Messaging.Mqtt.Tests.Integration.Fixtures;

namespace Caracal.Messaging.Mqtt.Tests.Integration;

[Trait("Category","Integration")]
[Collection("Mqtt collection")]
public sealed class A_Mqtt_Client: IDisposable
{
    private readonly Message _message;
    private readonly MqttConnection _connection = new (new MqttConnectionString{Port = 1999});
    private readonly Topic _topic = new () { Path = "test/integration/test1" };
    private readonly string _originalMessage = $"Request {Random.Shared.Next(1, 500)}";

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
    
    [Fact(Timeout = 5000)]
    public async Task Should_Subscribe_And_Publish_To_A_Topic()
    {
        // Arrange
        var messageFromSubscription = string.Empty;
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token;
        
        //Act
        var subscriptionResult = await _sut.SubscribeAsync(_topic, cancellationToken).ConfigureAwait(false);
        await _sut.PublishAsync(_message, cancellationToken).ConfigureAwait(false);
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
    public async Task Should_Publish_A_Command_To_A_Topic_And_Handle_The_Response()
    {
        // Arrange
        var requestMessage = $"Request {Random.Shared.Next(1, 500)}";
        var responseTask = PublishResponseAsync();
        var commandTask = PublishCommandAsync(requestMessage);
        
        // Act
        await Task.WhenAll(commandTask, responseTask).ConfigureAwait(false);
        var response = await commandTask.ConfigureAwait(false);
        
        // Assert
        response.Should().Be($"Response {requestMessage}");
    }
    
    public void Dispose() => _sut.Dispose();

    [ExcludeFromCodeCoverage]
    private async Task PublishResponseAsync()
    {
        await Task.Yield();
        var topic = new Topic { Path = $"test/command" };
        var responseTopic = new Topic { Path = "test/response" };
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token;

        var subscriptionResult = await _sut.SubscribeAsync(topic, cancellationToken).ConfigureAwait(false);
        using var subscription = subscriptionResult.Value!;

        var responseMsg = string.Empty;
        await foreach (var m in subscription.GetNextAsync(TimeSpan.FromSeconds(1)).ConfigureAwait(false))
        {
            responseMsg = $"Response {m.Value.Payload.GetString()}";
            break;
        }

        var message = new Message { Topic = responseTopic, Payload = responseMsg.GetBytes() };
        await _sut.PublishAsync(message, cancellationToken).ConfigureAwait(false);
        
        while (_connection.Client.PendingApplicationMessagesCount > 0)
            await Task.Delay(100, CancellationToken.None).ConfigureAwait(false);
    }

    [ExcludeFromCodeCoverage]
    private async Task<string> PublishCommandAsync(string msgString)
    {
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token;
        await Task.Delay(200, cancellationToken).ConfigureAwait(false);
        
        var topic = new Topic { Path = $"test/command" };
        var responseTopic = new Topic { Path = "test/response" };

        var message = new Message { Topic = topic, Payload = msgString.GetBytes() };
        var subscriptionResult = await _sut.PublishCommandAsync(message, responseTopic, CancellationToken.None).ConfigureAwait(false);
        using var subscription = subscriptionResult.Value!;
        await foreach (var m in subscription.GetNextAsync(TimeSpan.FromSeconds(1)).ConfigureAwait(false))
            return m.Value.Payload.GetString();
        
        return string.Empty;
    }
}