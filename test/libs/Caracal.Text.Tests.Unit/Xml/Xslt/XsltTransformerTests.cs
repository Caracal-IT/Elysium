using Caracal.Text.Xml.Xslt;

namespace Caracal.Text.Tests.Unit.Xml.Xslt;

public sealed class XsltTransformerTests
{
    [Fact]
    public void ASimpleXmlString_ShouldBeTransformed() {
        const string xml = ObjectStrings.SimpleXml;
        const string xslt = ObjectStrings.SimpleXslt;
        const string expected = "<Employee><FirstName>John</FirstName><Surname>Doe</Surname></Employee>";

        var result = XsltTransformer.Transform(xml, xslt);
        
        result.Should().Be(expected);
    }
}