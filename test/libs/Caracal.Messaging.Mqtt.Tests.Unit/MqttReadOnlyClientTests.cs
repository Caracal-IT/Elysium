using System.Text;
using Caracal.Lang;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.External.RxMQTT.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

public class MqttReadOnlyClientTests
{

    [Fact]
    public async Task Test7()
    {
        await Task.Delay(0);

        var connectionString = new MqttConnectionString();
        var mqttClient = new MqttFactory().CreateManagedMqttClient();
        var connection = new MqttConnection(mqttClient, connectionString);
        var client = new MqttReadOnlyClient(connection);
        var topic = new Topic
        {
            Path = $"test/1",
            QualityOfServiceLevel = 1

        };

        var subscription = await client.SubscribeAsync(topic, CancellationToken.None);
        
        var b = "";
        if (subscription.Value != null)
        {
            await foreach (var m in subscription.Value.GetNextAsync(TimeSpan.FromSeconds(10), CancellationToken.None).ConfigureAwait(false))
            {
                b += Encoding.UTF8.GetString(m.Value.Payload);
            }
        }
    }
    
    [Fact]
    public async Task Test6()
    {
        var connectionString = new MqttConnectionString();
        var mqttClient = new MqttFactory().CreateManagedMqttClient();
        var connection = new MqttConnection(mqttClient, connectionString);
        var client = new MqttReadOnlyClient(connection);
        
        var test4 = Test4(client);
        var test5 = Test5(client);
        
        await Task.WhenAll(test4, test5).ConfigureAwait(false);
    }
    
   // [Fact]
    public async Task Test5(MqttReadOnlyClient client)
    {
        await Task.Delay(0);

        var connectionString = new MqttConnectionString();
        var mqttClient = new MqttFactory().CreateManagedMqttClient();
        var connection = new MqttConnection(mqttClient, connectionString);
        var client2 = new MqttReadOnlyClient(connection);
        var topic = new Topic
        {
            Path = $"test/2",
            QualityOfServiceLevel = 1

        };

        var subscription = await client.SubscribeAsync(topic, CancellationToken.None);
        
        var b = "";
        if (subscription.Value != null)
        {
            await foreach (var m in subscription.Value.GetNextAsync(TimeSpan.FromSeconds(30), CancellationToken.None).ConfigureAwait(false))
                b += Encoding.UTF8.GetString(m.Value.Payload);
        }
    }
    
   // [Fact]
    public async Task Test4(MqttReadOnlyClient client)
    {
        await Task.Delay(0);

        var connectionString = new MqttConnectionString();
        var mqttClient = new MqttFactory().CreateManagedMqttClient();
        var connection = new MqttConnection(mqttClient, connectionString);
        var client2 = new MqttReadOnlyClient(connection);
        var topic = new Topic
        {
            Path = $"test/1",
            QualityOfServiceLevel = 1

        };

        var subscription = await client.SubscribeAsync(topic, CancellationToken.None);
        
        var b = "";
        if (subscription.Value != null)
        {
            await foreach (var m in subscription.Value.GetNextAsync(TimeSpan.FromSeconds(30), CancellationToken.None).ConfigureAwait(false))
                b += Encoding.UTF8.GetString(m.Value.Payload);
        }
    }

    [Fact]
    public async Task Test11()
    {
        await Task.Delay(0);
      
        var client = new MqttFactory().CreateRxMqttClient();
        var options = new ManagedMqttClientOptionsBuilder().WithClientOptions( new MqttClientOptionsBuilder().WithTcpServer("127.0.0.1", 1883).Build()).Build();
        
        await client.StartAsync(options).ConfigureAwait(false);
        bool received = false;
        var subscription = client
            .Connect("test/1")
            .SelectPayload()
            .Subscribe(payload =>
            {
                received = true;
            });
        
        
        for (int i = 1; i < 100; i++)
            await Task.Delay(1000);
    }
    

    [Fact]
    public async Task Test1()
    {
        await Task.Delay(0);
        
        var connectionString = new MqttConnectionString();
        var mqttClient = new MqttFactory().CreateManagedMqttClient();
        var connection = new MqttConnection(mqttClient, connectionString);

        var conn = await connection.ConnectAsync();

        if (conn.IsFaulted)
            return;

        var connectionString2 = new MqttConnectionString();
        var mqttClient2 = new MqttFactory().CreateManagedMqttClient();
        var connection2 = new MqttConnection(mqttClient, connectionString);

        var conn2 = await connection2.ConnectAsync();

        if (conn2.IsFaulted)
            return;
        
        var client = ((MqttConnectionDetails)conn.Value!).MqttClient;
        
        
        
        client.ApplicationMessageReceivedAsync +=  sender =>
        {
            var appMsg = sender.ApplicationMessage;
            var payload = Encoding.UTF8.GetString(appMsg.Payload);
            Console.WriteLine($"Topic: {appMsg.Topic} Payload: {payload}");
            
            return Task.CompletedTask;
        };
        
        client.ApplicationMessageProcessedAsync += sender =>
        {
            
            return Task.CompletedTask;
        };
        
        client.ApplicationMessageSkippedAsync += sender =>
        {
            
            return Task.CompletedTask;
        };
        
        client.SynchronizingSubscriptionsFailedAsync += sender =>
        {
            
            return Task.CompletedTask;
        };
        
        var client2 = ((MqttConnectionDetails)conn.Value!).MqttClient;
        
        client2.ApplicationMessageReceivedAsync +=  sender =>
        {
            var appMsg = sender.ApplicationMessage;
            var payload = Encoding.UTF8.GetString(appMsg.Payload);
            Console.WriteLine($"Topic: {appMsg.Topic} Payload: {payload}");
            return Task.CompletedTask;
        };
        
        await client.SubscribeAsync("test/1");
        await client2.SubscribeAsync("test/2");
        
        for (int i = 1; i < 100; i++)
            await Task.Delay(1000);
    }

    [Fact]
    public async Task Test2()
    {
        await Task.Delay(0, CancellationToken.None);
        var connectionString = new MqttConnectionString();
        var mqttClient = new MqttFactory().CreateManagedMqttClient();
        var connection = new MqttConnection(mqttClient, connectionString);
        var client = new MqttWriteOnlyClient(connection);
        
        var message = new Message
        {
            Topic = new Topic
            {
                Path = $"test/{Guid.NewGuid()}",
                QualityOfServiceLevel = 1,
                Retain = true
            },
            Payload = "Hello World"u8.ToArray()
        };

        var result = await client.PublishAsync(message, CancellationToken.None);

        await connection.DisconnectAsync(CancellationToken.None);
    }

    [Fact]
    public async Task SubscriptionToTopic_ShouldReturnSubscription()
    {
        var mqttClient = Substitute.For<IManagedMqttClient>();
        var conn = Substitute.For<IConnection>();
        var connDetails  = new MqttConnectionDetails { MqttClient = mqttClient };
       
        conn.ConnectAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new Result<ConnectionDetails>(connDetails)));
        
        var sut = new MqttReadOnlyClient(conn);
        var result = await sut.SubscribeAsync(new Topic{Path = "Mock Name"}, CancellationToken.None);
        
        result.Value.Should().BeOfType<MqttSubscription>();
    }
}