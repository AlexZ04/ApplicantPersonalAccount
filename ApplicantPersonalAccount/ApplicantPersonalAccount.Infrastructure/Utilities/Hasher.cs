using System.Web.Helpers;

namespace ApplicantPersonalAccount.Infrastructure.Utilities
{
    public class Hasher
    {
        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public bool CheckPassword(string hashedPassword, string enteredPassword)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, enteredPassword);
        }
    }
}
