// ReSharper disable InconsistentNaming

using Caracal.Text.Tests.Unit.Utils;
using Caracal.Text.Xml;
using Caracal.Text.Xml.Xslt;

namespace Caracal.Text.Tests.Unit.Xml.Xslt;

[Trait("Category", "Unit")]
public sealed class The_Xslt_Transformer
{
    [Fact]
    public void Should_Transform_A_Xml_String_Without_Nested_Items_Or_Methods_Into_A_New_Xml_String()
    {
        const string xml = ObjectStrings.SimpleXml;
        const string xslt = ObjectStrings.SimpleXslt;

        var result = XsltTransformer.Transform(xml, xslt);
        var employee = result.FromXml<Employee>();

        employee.Should().BeEquivalentTo(Employee.Default);
    }

    [Fact]
    public void Should_Transform_A_Xml_String_With_Nested_Items_Or_Methods_Into_A_New_Xml_String()
    {
        var extensions = new Dictionary<string, object>
        {
            { "utility:hash/v1", HashUtility.Instance }
        };

        const string xml = ObjectStrings.ComplexXmlWithAttributesAndLists;
        const string xslt = ObjectStrings.ComplexXslt;

        var result = XsltTransformer.Transform(xml, xslt, extensions);
        var employee = result.FromXml<Employee>();

        employee.Should().BeEquivalentTo(Employee.Complex);
    }
}