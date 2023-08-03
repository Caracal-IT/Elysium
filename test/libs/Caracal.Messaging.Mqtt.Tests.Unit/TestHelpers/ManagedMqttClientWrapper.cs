using System.Diagnostics.CodeAnalysis;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;

namespace Caracal.Messaging.Mqtt.Tests.Unit.TestHelpers;

[ExcludeFromCodeCoverage]
public sealed class ManagedMqttClientWrapper : IManagedMqttClient
{
    private readonly IManagedMqttClient _managedMqttClientImplementation;
    private readonly List<Func<MqttApplicationMessageReceivedEventArgs, Task>> _events = new();
    
    public async Task SendApplicationMessageAsync(string clientId, string topic, byte[] payload)
    {
        var args = CreateEventArgs(clientId, topic, payload);
        
        foreach (var eventHandler in _events)
            await eventHandler(args);
    }
    
    public event Func<MqttApplicationMessageReceivedEventArgs, Task>? ApplicationMessageReceivedAsync
    {
        add
        {  
            if(value != null)
                _events.Add(value);
            
            _managedMqttClientImplementation.ApplicationMessageReceivedAsync += value;
        }
        remove
        {
            if (value != null)
            {
                var item = _events.FirstOrDefault(t => t == value);
                if (item != null)
                    _events.Remove(item);
            }

            _managedMqttClientImplementation.ApplicationMessageReceivedAsync -= value;
        }
    }
    
    private static MqttApplicationMessageReceivedEventArgs CreateEventArgs(string clientId, string topic, byte[] payload) =>
        new(
            clientId,
            new MqttApplicationMessage{ Topic = topic,PayloadSegment = payload,},
            new MqttPublishPacket(),
            null);      

    public ManagedMqttClientWrapper(IManagedMqttClient managedMqttClientImplementation) =>
        _managedMqttClientImplementation = managedMqttClientImplementation;

    public void Dispose() => _managedMqttClientImplementation.Dispose();

    public Task EnqueueAsync(MqttApplicationMessage applicationMessage) =>
        _managedMqttClientImplementation.EnqueueAsync(applicationMessage);

    public Task EnqueueAsync(ManagedMqttApplicationMessage applicationMessage) =>
        _managedMqttClientImplementation.EnqueueAsync(applicationMessage);

    public Task PingAsync(CancellationToken cancellationToken = new CancellationToken()) =>
        _managedMqttClientImplementation.PingAsync(cancellationToken);

    public Task StartAsync(ManagedMqttClientOptions options) => _managedMqttClientImplementation.StartAsync(options);
    public Task StopAsync(bool cleanDisconnect = true) => _managedMqttClientImplementation.StopAsync(cleanDisconnect);

    public Task SubscribeAsync(ICollection<MqttTopicFilter> topicFilters) =>
        _managedMqttClientImplementation.SubscribeAsync(topicFilters);

    public Task UnsubscribeAsync(ICollection<string> topics) =>
        _managedMqttClientImplementation.UnsubscribeAsync(topics);

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
}