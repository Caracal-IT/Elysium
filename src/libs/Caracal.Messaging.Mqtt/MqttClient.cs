using Caracal.Lang;

namespace Caracal.Messaging.Mqtt;

public sealed class MqttClient: IClient 
{
    private readonly MqttConnection _connection;
    private readonly MqttReadOnlyClient _readOnlyClient;
    private readonly MqttWriteOnlyClient _writeOnlyClient;
    private ISubscription? _subscription;

    public MqttClient(MqttConnection connection)
    {
        _connection = connection;
        _readOnlyClient = new MqttReadOnlyClient(connection);
        _writeOnlyClient = new MqttWriteOnlyClient(connection);
    }

    public Task<Result<ISubscription>> SubscribeAsync(Topic topic, CancellationToken cancellationToken = default) =>
        _readOnlyClient.SubscribeAsync(topic, cancellationToken);
    

    public Task<Result<bool>> PublishAsync(Message message, CancellationToken cancellationToken = default) =>
        _writeOnlyClient.PublishAsync(message, cancellationToken);
    
    public async Task<Result<ISubscription>> PublishCommandAsync(Message message, Topic responseTopic, CancellationToken cancellationToken = default)
    {
        var command  = message with{ ResponseTopic = responseTopic with{ Retain = false } };
        var result = await SubscribeAsync(responseTopic, cancellationToken).ConfigureAwait(false);
        await Task.Delay(100, cancellationToken);
        await PublishAsync(command, cancellationToken).ConfigureAwait(false);
        
        if (result.IsFaulted)
            return result;
        
        _subscription = result.Value!;
        
        return new Result<ISubscription>(_subscription);
    }

    public void Dispose()
    {
        _subscription?.Dispose();
        _connection.Dispose();
        _readOnlyClient.Dispose();
        _writeOnlyClient.Dispose();
    }
}