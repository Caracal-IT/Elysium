using Caracal.Text.Xml;

namespace Caracal.Text.Tests.Unit.Xml;

public class XmlExtensionsTests
{
    private const string SimpleXml = """
        <?xml version="1.0" encoding="utf-16"?>
        <User xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
          <FirstName>John</FirstName>
          <LastName>Doe</LastName>
          <Age>42</Age>
        </User>
        """;
    private static readonly User SimpleObj = new () { FirstName = "John", LastName = "Doe", Age = 42 };

    private const string ComplexXml = """
        <?xml version="1.0" encoding="utf-16"?>
        <User xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
          <FirstName>John</FirstName>
          <LastName>Doe</LastName>
          <Age>42</Age>
          <Address>
            <Street>Main</Street>
            <Number>123</Number>
          </Address>
        </User>
        """;
    private static readonly User ComplexObj = new () { FirstName = "John", LastName = "Doe", Age = 42, Address = new Address { Street = "Main", Number = 123 } };
    
    [Fact]
    public void ASimpleObject_ShouldBeSerialized() {
        var xml = SimpleObj.ToXml();
        xml.Should().Be(SimpleXml);
    }
    
    [Fact]
    public void AComplexObject_ShouldBeSerialized() {
        var xml = ComplexObj.ToXml();
        xml.Should().Be(ComplexXml);
    }

    [Fact]
    public void AXmlStringWithoutSubTypes_ShouldBeDeserialized() {
        var obj = SimpleXml.FromXml<User>();

        obj.Should().BeEquivalentTo(SimpleObj);
    }
}