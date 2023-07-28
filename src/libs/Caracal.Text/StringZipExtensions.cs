﻿using System.IO.Compression;
using System.Text;

namespace Caracal.Text;

public static class StringZipExtensions
{
    public static byte[] Compress(this string input)
    {
        var buffer = Encoding.UTF8.GetBytes(input);
        using var memoryStream = new MemoryStream();
        using var stream = new GZipStream(memoryStream, CompressionMode.Compress);
        stream.Write(buffer, 0, buffer.Length);
        stream.Close();
        
        return memoryStream.ToArray();
    }
    
    public static string CompressToBase64(this string input) => Convert.ToBase64String(input.Compress());
    
    public static string Decompress(this byte[] input)
    {
        using var inputMemoryStream = new MemoryStream(input);
        using var outputMemoryStream = new MemoryStream();
        using var stream = new GZipStream(inputMemoryStream, CompressionMode.Decompress);
        stream.CopyTo(outputMemoryStream);
        stream.Close();
        
        return Encoding.UTF8.GetString(outputMemoryStream.ToArray());
    }
    
    public static string DecompressFromBase64(this string input) => Convert.FromBase64String(input).Decompress();
}