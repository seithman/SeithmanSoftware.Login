using System.Security.Cryptography;
using SeithmanSoftware.ByteArrayUtilities;

namespace SeithmanSoftware.Login.Controller.Helpers
{
    internal static class CryptoHelper
    {
        public static void HashNewPassword(string password, out byte[] salt, out byte[] hash)
        {
            salt = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            using var hasher = new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = 400
            };
            hash = hasher.GetBytes(64);
        }

        public static void HashPassword(string password, byte[] salt, out byte[] hash)
        {
            using var hasher = new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = 400
            };
            hash = hasher.GetBytes(64);
        }

        public static string CreateToken()
        {
            byte[] token = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(token);
            return token.ToBase64UrlEncoded();
        }
    }
}
