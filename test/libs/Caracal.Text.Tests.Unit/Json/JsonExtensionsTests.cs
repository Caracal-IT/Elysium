using Caracal.Text.Json;

namespace Caracal.Text.Tests.Unit.Json;

public sealed class JsonExtensionsTests
{
    [Fact]
    public void ASimpleObject_ShouldBeSerialized() {
        var json = User.Default.ToJson();
        json.Should().Be(ObjectStrings.SimpleJson);
    }
    
    [Fact]
    public void AComplexObject_ShouldBeSerialized() {
        var json = User.DefaultWithAddress.ToJson();
        json.Should().Be(ObjectStrings.ComplexJson);
    }
    
    [Fact]
    public void ASimpleObject_ShouldBeDeserialized() {
        var obj = ObjectStrings.SimpleJson.FromJson<User>();

        obj.Should().BeEquivalentTo(User.Default);
    }
    
    [Fact]
    public void AComplexObject_ShouldBeDeserialized() {
        var obj = ObjectStrings.ComplexJson.FromJson<User>();

        obj.Should().BeEquivalentTo(User.DefaultWithAddress);
    }

    [Fact]
    public void AComplexXmlString_ShouldBeConvertedToJson()
    {
        var json = ObjectStrings.ComplexXml.ToJsonFromXml();

        json.Should().Be(ObjectStrings.JsonFromXml);
    }
}