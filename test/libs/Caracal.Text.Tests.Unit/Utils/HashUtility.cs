using System.Security.Cryptography;

namespace Caracal.Text.Tests.Unit.Utils;

public sealed class HashUtility {
    
    public static HashUtility Instance { get; } = new();
    
    private HashUtility() {}
    
    public static string Hash256(string strToHash) =>
        SHA256.HashData(strToHash.GetBytes())
              .Aggregate(string.Empty, (current, theByte) => $"{current}{theByte:x2}");
}