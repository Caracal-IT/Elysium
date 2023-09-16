using Caracal.Text.Json;

// ReSharper disable InconsistentNaming
namespace Caracal.Text.Tests.Unit.Json;

[Trait("Category", "Unit")]
public sealed class The_Json_Extensions
{
    [Fact]
    public void Should_Serialize_An_Object_Without_Nested_Objects_To_A_Json_String()
    {
        var json = User.Default.ToJson();
        json.Should().Be(ObjectStrings.SimpleJson);
    }

    [Fact]
    public void Should_Serialize_An_Object_With_Nested_Objects_To_A_Json_String()
    {
        var json = User.DefaultWithAddress.ToJson();
        json.Should().Be(ObjectStrings.ComplexJson);
    }

    [Fact]
    public void Should_Deserialize_A_Json_String_Without_Nested_Items_To_An_Object()
    {
        var obj = ObjectStrings.SimpleJson.FromJson<User>();

        obj.Should().BeEquivalentTo(User.Default);
    }

    [Fact]
    public void Should_Deserialize_A_Json_String_With_Nested_Items_To_An_Object()
    {
        var obj = ObjectStrings.ComplexJson.FromJson<User>();

        obj.Should().BeEquivalentTo(User.DefaultWithAddress);
    }

    [Fact]
    public void Should_Serialize_An_Xml_String_With_Nested_Item_To_A_Json_String()
    {
        var json = ObjectStrings.ComplexXml.ToJsonFromXml();

        json.Should().Be(ObjectStrings.JsonFromXml);
    }
}