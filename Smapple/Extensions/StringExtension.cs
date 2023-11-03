using System.Security.Cryptography;
using System.Text;

namespace Smapple.Extensions;

public static class StringExtension
{

    public static byte[] GetHashBytes(string inputString)
    {
        using (HashAlgorithm algorithm = SHA256.Create())
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static string GetHash(this string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHashBytes(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}