using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace ApplicantPersonalAccount.Infrastructure.Utilities
{
    public class Hasher
    {
        public static string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public static bool CheckPassword(string hashedPassword, string enteredPassword)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, enteredPassword);
        }

        public static string HashFilename(string filename)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes("Hash")));
        }
    }
}
