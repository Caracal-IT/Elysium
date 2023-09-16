using System.IO.Compression;

namespace Caracal.Text;

public static class StringZipExtensions
{
    public static byte[] Compress(this string input)
    {
        var buffer = input.GetBytes();
        using var memoryStream = new MemoryStream();
        using var stream = new GZipStream(memoryStream, CompressionMode.Compress);
        stream.Write(buffer, 0, buffer.Length);
        stream.Close();

        return memoryStream.ToArray();
    }

    public static string CompressToBase64(this string input)
    {
        return Convert.ToBase64String(input.Compress());
    }

    public static string Decompress(this byte[] input)
    {
        using var inputMemoryStream = new MemoryStream(input);
        using var outputMemoryStream = new MemoryStream();
        using var stream = new GZipStream(inputMemoryStream, CompressionMode.Decompress);
        stream.CopyTo(outputMemoryStream);
        stream.Close();

        return outputMemoryStream.ToArray().GetString();
    }

    public static string DecompressFromBase64(this string input)
    {
        return Convert.FromBase64String(input).Decompress();
    }
}