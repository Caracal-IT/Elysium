using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Caracal.Messaging.Mqtt.Tests.Integration.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class MqttFixture: IAsyncLifetime
{
    private IContainer _container = null!;
    
    public async Task InitializeAsync()
    {
        _container = new ContainerBuilder()
            .WithName(Guid.NewGuid().ToString("D"))
            .WithImage("hivemq/hivemq-ce:latest")
            .WithPortBinding(1883, 1883)
            .WithEnvironment(new Dictionary<string, string>()
            {
                { "JAVA_OPTS", "-XX:+UnlockExperimentalVMOptions -XX:InitialRAMPercentage=30 -XX:MaxRAMPercentage=80 -XX:MinRAMPercentage=30" }
            })
            .Build();

       await _container.StartAsync().ConfigureAwait(false);

       string output;
       string error;
       
       do
       {
           (output, error) = await _container.GetLogsAsync().ConfigureAwait(false);

           await Task.Delay(100).ConfigureAwait(false);
       } while (string.IsNullOrWhiteSpace(error) && !output.Contains("Started HiveMQ", StringComparison.InvariantCultureIgnoreCase));
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync().ConfigureAwait(false);
        await _container.DisposeAsync().ConfigureAwait(false);
    }
}

[CollectionDefinition("Mqtt collection")]
public class MqttCollection : ICollectionFixture<MqttFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}