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
    
    private Exception? _lastException = null;
    
    
    public Exception? LastException => _lastException;

    public MqttSubscription(MqttConnectionDetails connectionDetails, Topic topic)
    {
        _connectionDetails = connectionDetails;
        _topic = topic;
    }

    public Task<Result<bool>> UnsubscribeAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Result<bool>(true));
    }

    public async IAsyncEnumerable<Result<Message>> GetNextAsync(TimeSpan timeoutDuration, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        var channel = Channel.CreateUnbounded<MqttApplicationMessageReceivedEventArgs>();

        await _connectionDetails.MqttClient.SubscribeAsync(new List<MqttTopicFilter>(new[]
        {
            new MqttTopicFilter { Topic = _topic.Path, QualityOfServiceLevel = (MqttQualityOfServiceLevel) _topic.QualityOfServiceLevel },
        }));

        Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            channel.Writer.TryWrite(args);
            return Task.CompletedTask;
        }

        _connectionDetails.MqttClient.ApplicationMessageReceivedAsync += OnApplicationMessageReceivedAsync;

        try
        {
            var combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(timeoutDuration).Token);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                MqttApplicationMessageReceivedEventArgs item;
                try
                {
                    if (!await channel.Reader.WaitToReadAsync(combinedCancellationTokenSource.Token).ConfigureAwait(false))
                        break;

                    item = await channel.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (TimeoutException ex) { _lastException= ex; break; }
                catch(TaskCanceledException ex) { _lastException= ex; break; }
                catch (OperationCanceledException ex) { _lastException= ex; break; }

                if (item.ApplicationMessage.Topic != _topic.Path) continue;
                
                yield return Result(item);
                
                combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(timeoutDuration).Token);
            }
        }
        finally
        {
            // Unsubscribe the event handler when the iteration is complete.
            _connectionDetails.MqttClient.ApplicationMessageReceivedAsync -= OnApplicationMessageReceivedAsync;
            channel.Writer.Complete();
        }

        Result<Message> Result(MqttApplicationMessageReceivedEventArgs item)
        {
            return new Result<Message>(
                new Message
                {
                    Payload = item.ApplicationMessage.PayloadSegment.ToArray(),
                    Topic = new Topic { Path = item.ApplicationMessage.Topic }
                });
        }
    }
    
    public IAsyncEnumerable<Result<Message>> GetNextAsync(CancellationToken cancellationToken = default) => 
        GetNextAsync(TimeSpan.FromDays(370), cancellationToken);
}