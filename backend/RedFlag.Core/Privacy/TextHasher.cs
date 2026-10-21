using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RedFlag.Core.Privacy;

public static class TextHasher
{
    public static string Hash(string text)
    {
        var normalized = Regex.Replace(text.Trim().ToLowerInvariant(), @"\s+", " ");
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}