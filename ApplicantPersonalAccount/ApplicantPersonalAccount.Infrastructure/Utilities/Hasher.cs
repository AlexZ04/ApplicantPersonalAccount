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
            return filename;
        }
    }
}
