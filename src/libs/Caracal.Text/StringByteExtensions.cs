using System.Text;

namespace Caracal.Text;

public static class StringByteExtensions
{
    public static byte[] GetBytes(this string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }

    public static string GetString(this byte[] value)
    {
        return Encoding.UTF8.GetString(value);
    }
}