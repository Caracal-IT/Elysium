// ReSharper disable InconsistentNaming
using Caracal.Text.Xml;

namespace Caracal.Text.Tests.Unit.Xml;

[Trait("Category","Unit")]
public sealed class The_Xml_Extensions
{
    [Fact]
    public void Should_Serialize_An_Object_Without_Nested_Objects_Into_An_Xml_String() {
        var xml = User.Default.ToXml();
        xml.Should().Be(ObjectStrings.SimpleXml);
    }
    
    [Fact]
    public void Should_Serialize_An_Object_With_Nested_Objects_Into_An_Xml_String() {
        var xml = User.DefaultWithAddress.ToXml();
        xml.Should().Be(ObjectStrings.ComplexXml);
    }

    [Fact]
    public void Should_Deserialize_An_Xml_String_Without_Nested_Items_Into_An_Object() {
        var obj = ObjectStrings.SimpleXml.FromXml<User>();

        obj.Should().BeEquivalentTo(User.Default);
    }
    
    [Fact]
    public void  Should_Deserialize_An_Xml_String_With_Nested_Items_Into_An_Object() {
        var obj = ObjectStrings.ComplexXml.FromXml<User>();

        obj.Should().BeEquivalentTo(User.DefaultWithAddress);
    }
    
    [Fact]
    public void Should_Serialize_A_Json_String_Into_An_Xml_String()
    {
        var xml = ObjectStrings.ComplexJson.ToXmlFromJson(nameof(User));
        var expectedUser = xml.FromXml<User>();
        
        expectedUser.Should().BeEquivalentTo(User.DefaultWithAddress);
    }
}