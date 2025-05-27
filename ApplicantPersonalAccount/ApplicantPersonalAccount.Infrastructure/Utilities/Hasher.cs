using System.Security.Cryptography;
using System.Text;

namespace ApplicantPersonalAccount.Infrastructure.Utilities
{
    public class Hasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool CheckPassword(string hashedPassword, string enteredPassword)
        {
                    
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);

        }

        public static string HashFilename(string filename)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(filename)));
        }
    }
}
