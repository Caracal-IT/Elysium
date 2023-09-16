using System.Security.Cryptography;

namespace Caracal.Text.Tests.Unit.Utils;

public sealed class HashUtility
{
    private HashUtility()
    {
    }

    public static HashUtility Instance { get; } = new();

    public static string Hash256(string strToHash)
    {
        return SHA256.HashData(strToHash.GetBytes())
            .Aggregate(string.Empty, (current, theByte) => $"{current}{theByte:x2}");
    }
}