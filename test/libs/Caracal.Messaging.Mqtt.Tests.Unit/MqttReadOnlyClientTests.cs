using System.Text;
using Xunit.Abstractions;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

public class MqttReadOnlyClientTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public MqttReadOnlyClientTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Test7()
    {
        await Task.Delay(100);
        using var connection = new MqttConnection();
        using var client = new MqttReadOnlyClient(connection);
        
        var topic = new Topic { Path = $"test/1" };
        var subscription = await client.SubscribeAsync(topic, CancellationToken.None);

        var b = "";
        if (subscription.Value != null)
        {
            await foreach (var m in subscription.Value.GetNextAsync(TimeSpan.FromSeconds(5), CancellationToken.None).ConfigureAwait(false))
                b += Encoding.UTF8.GetString(m.Value.Payload);
        }
    }

    //[Fact]
    public async Task Test8()
    {
        await Task.Delay(100);
        var result = string.Empty;
        await using var connection = new MqttConnection();
        using var client = new MqttClient(connection);

        var topic = new Topic { Path = $"test/command" };
        var responseTopic = new Topic { Path = $"test/response" };
        var message = new Message { Topic = topic, Payload = Encoding.UTF8.GetBytes($"Request {Random.Shared.Next(1, 500)}") };

        var subscription = await client.PublishCommandAsync(message, responseTopic, CancellationToken.None);
        await foreach (var m in subscription.Value!.GetNextAsync(TimeSpan.FromSeconds(5), CancellationToken.None).ConfigureAwait(false))
        {
            result = Encoding.UTF8.GetString(m.Value.Payload);
            break;
        }
        
        _testOutputHelper.WriteLine(result);

        result.Should().Contain("Version");
    }

    public async Task Test9()
    {
        await using var connection = new MqttConnection();
        using var writeOnlyClient = new MqttWriteOnlyClient(connection);
        using var readOnlyClient = new MqttReadOnlyClient(connection);

        var topic = new Topic { Path = $"test/command" };
        var subscription = await readOnlyClient.SubscribeAsync(topic);
        
        await foreach (var m in subscription.Value!.GetNextAsync(TimeSpan.FromSeconds(50), CancellationToken.None).ConfigureAwait(false))
        {
            var topic2 = m.Value!.ResponseTopic??new Topic{Path = "Unknown"};
            var message = new Message { Topic = topic2, Payload = Encoding.UTF8.GetBytes($"Version {Random.Shared.Next(1, 500)}") };
            await writeOnlyClient.PublishAsync(message);
            break;
        }
    }

    [Fact]
    public async Task Test10()
    {
        await Task.WhenAll(new List<Task> { Test8(), Test9() });
    }
}