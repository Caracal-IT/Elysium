using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Caracal.Text.Json;

public static class JsonExtensions
{
    public static string ToJson(this object obj) => Serialize(obj);
    public static T FromJson<T>(this string json) => Deserialize<T>(json) ?? throw new JsonException();
}