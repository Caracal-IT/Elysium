using Caracal.Text.Tests.Unit.Utils;
using Caracal.Text.Xml;
using Caracal.Text.Xml.Xslt;

namespace Caracal.Text.Tests.Unit.Xml.Xslt;

public sealed class XsltTransformerTests
{
    [Fact]
    public void ASimpleXmlString_ShouldBeTransformed() {
        const string xml = ObjectStrings.SimpleXml;
        const string xslt = ObjectStrings.SimpleXslt;
        
        var result = XsltTransformer.Transform(xml, xslt);
        var employee = result.FromXml<Employee>();
        
        employee.Should().BeEquivalentTo(Employee.Default);
    }
    
    [Fact]
    public void AXmlStringWithAttributesAndLists_ShouldBeTransformed() {
        var extensions = new Dictionary<string, object> {
            { "utility:hash/v1", new HashUtility() }
        };
        
        const string xml = ObjectStrings.ComplexXmlWithAttributesAndLists;
        const string xslt = ObjectStrings.ComplexXslt;
        
        var result = XsltTransformer.Transform(xml, xslt, extensions);
        var employee = result.FromXml<Employee>();

        employee.Should().BeEquivalentTo(Employee.Complex);
    }
}