using System.Text;
using Xunit.Abstractions;

namespace Caracal.Messaging.Mqtt.Tests.Unit;

[Trait("Category","Unit")]
public class MqttReadOnlyClientTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public MqttReadOnlyClientTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
}