using System.Runtime.CompilerServices;
using Caracal.Lang;
using MQTTnet.Client;
using System.Threading.Channels;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttSubscription: ISubscription
{
    private readonly MqttConnectionDetails _connectionDetails;
    private readonly Topic _topic;
    private readonly Channel<MqttApplicationMessageReceivedEventArgs> _channel;
    private readonly CancellationToken _cancellationToken;
    
    private Exception? _lastException;
    
    public Exception? LastException => _lastException;

    internal MqttSubscription(MqttConnectionDetails connectionDetails, Topic topic, CancellationToken cancellationToken = default)
    {
        _connectionDetails = connectionDetails;
        _topic = topic;
        _cancellationToken = cancellationToken;
        
        _channel = Channel.CreateUnbounded<MqttApplicationMessageReceivedEventArgs>();
    }

    private async Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg) => 
        await _channel.Writer.WriteAsync(arg, _cancellationToken);

    public async IAsyncEnumerable<Result<Message>> GetNextAsync(TimeSpan timeoutDuration)
    {
        await foreach (var message in GetMessagesFromChannelAsync(_channel, timeoutDuration, _cancellationToken))
            yield return message;
    }

    public async Task UnsubscribeAsync()
    {
        _connectionDetails.MqttClient!.ApplicationMessageReceivedAsync -= OnApplicationMessageReceivedAsync;
        await _connectionDetails.MqttClient!.UnsubscribeAsync(new List<string> { _topic.Path });
    }

    public IAsyncEnumerable<Result<Message>> GetNextAsync() => 
        GetNextAsync(TimeSpan.FromDays(370));
    
    public void Dispose()
    {
        _connectionDetails.MqttClient!.ApplicationMessageReceivedAsync -= OnApplicationMessageReceivedAsync;
        _channel.Writer.TryComplete();
        _connectionDetails.Dispose();
    }

    internal async Task SubscribeToTopicsAsync()
    {
        _connectionDetails.MqttClient!.ApplicationMessageReceivedAsync += OnApplicationMessageReceivedAsync;
        await _connectionDetails.MqttClient!.SubscribeAsync(new List<MqttTopicFilter>(new[]
        {
            new MqttTopicFilter { Topic = _topic.Path, QualityOfServiceLevel = (MqttQualityOfServiceLevel)_topic.QualityOfServiceLevel },
        })).ConfigureAwait(false);
    }

    private async IAsyncEnumerable<Result<Message>> GetMessagesFromChannelAsync(Channel<MqttApplicationMessageReceivedEventArgs> channel, TimeSpan timeoutDuration, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(timeoutDuration).Token);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            MqttApplicationMessageReceivedEventArgs? item;
            try
            {
                item = await GetMessageFromChannelAsync(channel, combinedCancellationTokenSource.Token).ConfigureAwait(false);
                if (item is null) break;
            }
            catch (TaskCanceledException ex) { _lastException = ex; break; }
            catch (OperationCanceledException ex) { _lastException = ex; break; }

            if (item.ApplicationMessage.Topic != _topic.Path) continue;

            yield return CreateResult(item);

            combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                new CancellationTokenSource(timeoutDuration).Token
            );
        }
    }

    private static Result<Message> CreateResult(MqttApplicationMessageReceivedEventArgs item)
    {
        return new Result<Message>(
            new Message
            {
                Payload = item.ApplicationMessage.PayloadSegment.ToArray(),
                Topic = new Topic { Path = item.ApplicationMessage.Topic },
                ResponseTopic = new Topic { Path = item.ApplicationMessage.ResponseTopic, Retain = false }
            });
    }

    private static async Task<MqttApplicationMessageReceivedEventArgs?> GetMessageFromChannelAsync(Channel<MqttApplicationMessageReceivedEventArgs> channel, CancellationToken cancellationToken)
    {
        if (!await channel.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
            return null;

        return await channel.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
    }
}