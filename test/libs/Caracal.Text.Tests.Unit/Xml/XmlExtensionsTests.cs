using Caracal.Text.Xml;

namespace Caracal.Text.Tests.Unit.Xml;

public class XmlExtensionsTests
{
    [Fact]
    public void ASimpleObject_ShouldBeSerialized() {
        var xml = User.Default.ToXml();
        xml.Should().Be(ObjectStrings.SimpleXml);
    }
    
    [Fact]
    public void AComplexObject_ShouldBeSerialized() {
        var xml = User.DefaultWithAddress.ToXml();
        xml.Should().Be(ObjectStrings.ComplexXml);
    }

    [Fact]
    public void AXmlStringWithoutSubTypes_ShouldBeDeserialized() {
        var obj = ObjectStrings.SimpleXml.FromXml<User>();

        obj.Should().BeEquivalentTo(User.Default);
    }
    
    [Fact]
    public void AXmlStringWithTypes_ShouldBeDeserialized() {
        var obj = ObjectStrings.ComplexXml.FromXml<User>();

        obj.Should().BeEquivalentTo(User.DefaultWithAddress);
    }
}