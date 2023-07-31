using System.Text;

namespace Caracal.Text;

public static class StringByteExtensions
{
    public static byte[] GetBytes(this string value) => Encoding.UTF8.GetBytes(value);
    public static string GetString(this byte[] value) => Encoding.UTF8.GetString(value);
}