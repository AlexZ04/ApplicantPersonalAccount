using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Common.Models.User
{
    public class PasswordEditModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
