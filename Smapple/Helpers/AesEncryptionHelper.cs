using System.Security.Cryptography;
using System.Text;

namespace Headstart.Common.Helpers
{

    public static class AesEncryptionHelper
    {
        private const string _encryptionKey = "SmappleVerySecureKey";

        public static string EncryptString(string input)
        {
            var plaintextBytes = System.Text.Encoding.UTF8.GetBytes(input);

            var passwordBytes = DeriveKeyFromPassword(_encryptionKey);

            var encryptor = Aes.Create();
            encryptor.Key = passwordBytes[..16];
            encryptor.IV = passwordBytes[16..];

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(plaintextBytes, 0, plaintextBytes.Length);
                    cs.FlushFinalBlock();
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static string DecryptString(string encrypted)
        {
            var encryptedBytes = Convert.FromBase64String(encrypted);

            var passwordBytes = DeriveKeyFromPassword(_encryptionKey);

            var encryptor = Aes.Create();
            encryptor.Key = passwordBytes[..16];
            encryptor.IV = passwordBytes[16..];

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                }

                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private static byte[] DeriveKeyFromPassword(string password)
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 32; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                emptySalt,
                iterations,
                hashMethod,
                desiredKeyLength);
        }
    }
}