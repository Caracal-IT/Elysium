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
}