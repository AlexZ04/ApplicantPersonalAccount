using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Staff.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required]
        public string? UserEmail { get; set; }
        [Display(Name = "Password")]
        [UIHint("password")]
        [Required]
        public string? Password { get; set; }
    }
}
