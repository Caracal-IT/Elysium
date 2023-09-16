using System.Xml;
using Newtonsoft.Json;
using static System.Text.Json.JsonSerializer;
using JsonException = System.Text.Json.JsonException;

namespace Caracal.Text.Json;

public static class JsonExtensions
{
    public static string ToJson(this object obj)
    {
        return Serialize(obj);
    }

    public static T FromJson<T>(this string json)
    {
        return Deserialize<T>(json) ?? throw new JsonException();
    }

    public static string ToJsonFromXml(this string xml)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        return JsonConvert.SerializeObject(xmlDoc);
    }
}