using Caracal.Text.Json;
using FluentAssertions;

namespace Caracal.Text.Tests.Unit.Json;

public class JsonExtensionsTests
{
    [Fact]
    public void ASimpleObject_ShouldBeSerialized() {
        var obj = new { Name = "John", Age = 42 };
        var json = obj.ToJson();
        json.Should().Be("{\"Name\":\"John\",\"Age\":42}");
    }
    
    [Fact]
    public void AComplexObject_ShouldBeSerialized() {
        var obj = new { Name = "John", Age = 42, Address = new { Street = "Main", Number = 123 } };
        var json = obj.ToJson();
        json.Should().Be("{\"Name\":\"John\",\"Age\":42,\"Address\":{\"Street\":\"Main\",\"Number\":123}}");
    }
}