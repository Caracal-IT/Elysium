// ReSharper disable InconsistentNaming

using MQTTnet.Client;
using MQTTnet.Formatter;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

[Trait("Category", "Unit")]
public class A_Mqtt_Connection_String
{
    [Fact]
    public void Should_Have_Valid_Defaults()
    {
        var connectionString = new MqttConnectionString();

        connectionString.Host.Should().Be("127.0.0.1");
        connectionString.Port.Should().Be(1883);
        connectionString.ClientId.Should().NotBeNullOrEmpty();
        connectionString.ClientId.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Should_Be_Able_To_Change_Defaults()
    {
        var connectionString = new MqttConnectionString
        {
            Host = "test",
            Port = 1234,
            ClientId = "test",
            ProtocolVersion = MqttProtocolVersion.V500
        };

        connectionString.Host.Should().Be("test");
        connectionString.Port.Should().Be(1234);
        connectionString.ClientId.Should().Be("test");
    }

    [Fact]
    public void Should_Build_Valid_Connection_String()
    {
        var connectionString = new MqttConnectionString
        {
            Host = "test",
            Port = 1234,
            ClientId = "test",
            ProtocolVersion = MqttProtocolVersion.V500
        };

        var options = connectionString.Build();

        options.ClientOptions.ProtocolVersion.Should().Be(MqttProtocolVersion.V500);
        options.ClientOptions.ClientId.Should().Be("test");
        options.ClientOptions.ChannelOptions.Should().NotBeNull();
        options.ClientOptions.ChannelOptions.Should().BeOfType<MqttClientTcpOptions>();
        options.ClientOptions.ChannelOptions.Should().BeOfType<MqttClientTcpOptions>();
        ((MqttClientTcpOptions)options.ClientOptions.ChannelOptions).Port.Should().Be(1234);
        ((MqttClientTcpOptions)options.ClientOptions.ChannelOptions).Server.Should().Be("test");
    }
}