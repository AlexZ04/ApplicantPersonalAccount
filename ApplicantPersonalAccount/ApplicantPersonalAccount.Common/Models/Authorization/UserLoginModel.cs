using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Common.Models.Authorization
{
    public class UserLoginModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
