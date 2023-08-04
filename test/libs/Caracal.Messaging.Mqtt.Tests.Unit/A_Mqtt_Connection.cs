// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using MQTTnet.Extensions.ManagedClient;
using NSubstitute.ExceptionExtensions;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

[Trait("Category","Unit")]
public class A_Mqtt_Connection
{
    private readonly IManagedMqttClient _client;
    private readonly MqttConnection _sut;
    
    public A_Mqtt_Connection()
    {
        _client = Substitute.For<IManagedMqttClient>();
        _sut = new MqttConnection(_client, new MqttConnectionString());
    }
    
    [Fact]
    public void Should_Have_Default_Client()
    {
        var connection = new MqttConnection(new MqttConnectionString());
        
        connection.Client.Should().NotBeNull();
        connection.Client.Should().BeAssignableTo<IManagedMqttClient>();
    }
    
    [Fact]
    public async Task Should_Return_Faulted_If_Connection_Failed()
    {
        _client.StartAsync(Arg.Any<ManagedMqttClientOptions>())
               .ThrowsForAnyArgs(new Exception("Connection failed"));
        
        var result = await _sut.ConnectAsync(CancellationToken.None).ConfigureAwait(false);
        
        result.IsFaulted.Should().BeTrue();
        result.Exception!.Message.Should().Be("Connection failed");
    }
    
    [Fact]
    public async Task Should_Return_Success_If_Connection_Succeeded()
    {
        var result = await _sut.ConnectAsync(CancellationToken.None).ConfigureAwait(false);
        
        result.IsFaulted.Should().BeFalse();
        result.Exception.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeAssignableTo<MqttConnectionDetails>();
    }
    
    [Fact]
    public void Should_Dispose_Client()
    {
        _sut.Dispose();
        
        _client.Received().Dispose();
    }
    
    /*
    [Fact]
    public async Task Should_Dispose_Async()
    {
        await _sut.DisposeAsync().ConfigureAwait(false);
        
        await _client.Received(1).StopAsync().ConfigureAwait(false);
        //await _client.Received().DisposeAsync().ConfigureAwait(false);
    }
    */
}