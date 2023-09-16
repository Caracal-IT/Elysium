using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace Caracal.Text.Xml.Xslt;

public sealed class XsltTransformer
{
    private readonly IDictionary<string, object> _args;
    private readonly string _xml;
    private readonly string _xslt;

    private XsltTransformer(string xml, string xslt, IDictionary<string, object>? args = default)
    {
        _xml = xml;
        _xslt = xslt;
        _args = args ?? new Dictionary<string, object>();
    }

    public static string Transform(string xml, string xslt, IDictionary<string, object>? extensions = null)
    {
        return new XsltTransformer(xml, xslt, extensions).Transform();
    }

    private string Transform()
    {
        var output = new StringBuilder();

        using var xsltReader = new XmlTextReader(new StringReader(_xslt));
        var transform = new XslCompiledTransform();
        transform.Load(xsltReader);

        using var reader = new XmlTextReader(new StringReader(_xml));
        using var writer = new XmlTextWriter(new StringWriter(output));
        transform.Transform(reader, GetXsltArguments(), writer);

        return output.ToString();
    }

    private XsltArgumentList GetXsltArguments()
    {
        var args = new XsltArgumentList();

        foreach (var (key, value) in _args)
            args.AddExtensionObject(key, value);

        return args;
    }
}