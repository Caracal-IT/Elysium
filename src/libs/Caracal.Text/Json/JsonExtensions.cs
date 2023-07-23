using System.Text.Json;
using System.Xml;
using static System.Text.Json.JsonSerializer;

namespace Caracal.Text.Json;

public static class JsonExtensions
{
    public static string ToJson(this object obj) => Serialize(obj);
    public static T FromJson<T>(this string json) => Deserialize<T>(json) ?? throw new JsonException();

    public static string ToJsonFromXml(this string xml)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        return Newtonsoft.Json.JsonConvert.SerializeObject(xmlDoc);
    }
}