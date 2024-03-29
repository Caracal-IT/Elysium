using System.Diagnostics.CodeAnalysis;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;

// ReSharper disable HeapView.ObjectAllocation.Possible
// ReSharper disable ClassNeverInstantiated.Global

namespace Caracal.Messaging.Mqtt.Tests.Unit.TestHelpers;

[ExcludeFromCodeCoverage]
public class ManagedMqttClientWrapper : IManagedMqttClient
{
    private readonly IManagedMqttClient _managedMqttClientImplementation;
    internal readonly List<Func<MqttApplicationMessageReceivedEventArgs, Task>> Events = new();

    public ManagedMqttClientWrapper(IManagedMqttClient managedMqttClientImplementation)
    {
        _managedMqttClientImplementation = managedMqttClientImplementation;
    }

    public event Func<MqttApplicationMessageReceivedEventArgs, Task>? ApplicationMessageReceivedAsync
    {
        add
        {
            if (value != null)
                Events.Add(value);

            _managedMqttClientImplementation.ApplicationMessageReceivedAsync += value;
        }
        remove
        {
            if (value != null)
            {
                var item = Events.FirstOrDefault(t => t == value);
                if (item != null)
                    Events.Remove(item);
            }

            _managedMqttClientImplementation.ApplicationMessageReceivedAsync -= value;
        }
    }

    public void Dispose()
    {
        _managedMqttClientImplementation.Dispose();
    }

    public Task EnqueueAsync(MqttApplicationMessage applicationMessage)
    {
        return _managedMqttClientImplementation.EnqueueAsync(applicationMessage);
    }

    public Task EnqueueAsync(ManagedMqttApplicationMessage applicationMessage)
    {
        return _managedMqttClientImplementation.EnqueueAsync(applicationMessage);
    }

    public Task PingAsync(CancellationToken cancellationToken = new())
    {
        return _managedMqttClientImplementation.PingAsync(cancellationToken);
    }

    public Task StartAsync(ManagedMqttClientOptions options)
    {
        return _managedMqttClientImplementation.StartAsync(options);
    }

    public Task StopAsync(bool cleanDisconnect = true)
    {
        return _managedMqttClientImplementation.StopAsync(cleanDisconnect);
    }

    public Task SubscribeAsync(ICollection<MqttTopicFilter> topicFilters)
    {
        return _managedMqttClientImplementation.SubscribeAsync(topicFilters);
    }

    public Task UnsubscribeAsync(ICollection<string> topics)
    {
        return _managedMqttClientImplementation.UnsubscribeAsync(topics);
    }

    public IMqttClient InternalClient => _managedMqttClientImplementation.InternalClient;
    public bool IsConnected => _managedMqttClientImplementation.IsConnected;

    public bool IsStarted => _managedMqttClientImplementation.IsStarted;

    public ManagedMqttClientOptions Options => _managedMqttClientImplementation.Options;

    public int PendingApplicationMessagesCount => _managedMqttClientImplementation.PendingApplicationMessagesCount;

    public event Func<ApplicationMessageProcessedEventArgs, Task>? ApplicationMessageProcessedAsync
    {
        add => _managedMqttClientImplementation.ApplicationMessageProcessedAsync += value;
        remove => _managedMqttClientImplementation.ApplicationMessageProcessedAsync -= value;
    }

    public event Func<ApplicationMessageSkippedEventArgs, Task>? ApplicationMessageSkippedAsync
    {
        add => _managedMqttClientImplementation.ApplicationMessageSkippedAsync += value;
        remove => _managedMqttClientImplementation.ApplicationMessageSkippedAsync -= value;
    }

    public event Func<MqttClientConnectedEventArgs, Task>? ConnectedAsync
    {
        add => _managedMqttClientImplementation.ConnectedAsync += value;
        remove => _managedMqttClientImplementation.ConnectedAsync -= value;
    }

    public event Func<ConnectingFailedEventArgs, Task>? ConnectingFailedAsync
    {
        add => _managedMqttClientImplementation.ConnectingFailedAsync += value;
        remove => _managedMqttClientImplementation.ConnectingFailedAsync -= value;
    }

    public event Func<EventArgs, Task>? ConnectionStateChangedAsync
    {
        add => _managedMqttClientImplementation.ConnectionStateChangedAsync += value;
        remove => _managedMqttClientImplementation.ConnectionStateChangedAsync -= value;
    }

    public event Func<MqttClientDisconnectedEventArgs, Task>? DisconnectedAsync
    {
        add => _managedMqttClientImplementation.DisconnectedAsync += value;
        remove => _managedMqttClientImplementation.DisconnectedAsync -= value;
    }

    public event Func<ManagedProcessFailedEventArgs, Task>? SynchronizingSubscriptionsFailedAsync
    {
        add => _managedMqttClientImplementation.SynchronizingSubscriptionsFailedAsync += value;
        remove => _managedMqttClientImplementation.SynchronizingSubscriptionsFailedAsync -= value;
    }

    public async Task SendApplicationMessageAsync(string clientId, string topic, byte[] payload)
    {
        var args = CreateEventArgs(clientId, topic, payload);

        foreach (var eventHandler in Events)
            await eventHandler(args);
    }

    private static MqttApplicationMessageReceivedEventArgs CreateEventArgs(string clientId, string topic, byte[] payload)
    {
        return new MqttApplicationMessageReceivedEventArgs(
            clientId,
            new MqttApplicationMessage { Topic = topic, PayloadSegment = payload },
            new MqttPublishPacket(),
            null);
    }
}