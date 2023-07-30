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
    
    private Exception? _lastException;
    
    
    public Exception? LastException => _lastException;

    public MqttSubscription(MqttConnectionDetails connectionDetails, Topic topic)
    {
        _connectionDetails = connectionDetails;
        _topic = topic;
    }

    public async IAsyncEnumerable<Result<Message>> GetNextAsync(TimeSpan timeoutDuration, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        
        var channel = Channel.CreateUnbounded<MqttApplicationMessageReceivedEventArgs>();

        await SubscribeToTopicsAsync();

        async Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args) => await channel.Writer.WriteAsync(args, cancellationToken);
        _connectionDetails.MqttClient!.ApplicationMessageReceivedAsync += OnApplicationMessageReceivedAsync;

        try
        {
            await foreach (var message in GetMessagesFromChannelAsync(channel, timeoutDuration, cancellationToken))
                yield return message;
        }
        finally
        {
            _connectionDetails.MqttClient!.ApplicationMessageReceivedAsync -= OnApplicationMessageReceivedAsync;
            channel.Writer.Complete();
        }
    }
    
    public IAsyncEnumerable<Result<Message>> GetNextAsync(CancellationToken cancellationToken = default) => 
        GetNextAsync(TimeSpan.FromDays(370), cancellationToken);

    private async Task SubscribeToTopicsAsync()
    {
        await _connectionDetails.MqttClient!.SubscribeAsync(new List<MqttTopicFilter>(new[]
        {
            new MqttTopicFilter { Topic = _topic.Path, QualityOfServiceLevel = (MqttQualityOfServiceLevel)_topic.QualityOfServiceLevel },
        }));
    }

    private async IAsyncEnumerable<Result<Message>> GetMessagesFromChannelAsync(Channel<MqttApplicationMessageReceivedEventArgs> channel, TimeSpan timeoutDuration, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(timeoutDuration).Token);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            MqttApplicationMessageReceivedEventArgs? item;
            try
            {
                item = await GetMessageFromChannelAsync(channel, combinedCancellationTokenSource.Token);
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
                Topic = new Topic { Path = item.ApplicationMessage.Topic }
            });
    }

    private static async Task<MqttApplicationMessageReceivedEventArgs?> GetMessageFromChannelAsync(Channel<MqttApplicationMessageReceivedEventArgs> channel, CancellationToken cancellationToken)
    {
        if (!await channel.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
            return null;

        return await channel.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
    }

    public void Dispose() => _connectionDetails.Dispose();
}