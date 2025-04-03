using ApplicantPersonalAccount.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApplicantPersonalAccount.Common.Models.User
{
    public class UserEditModel
    {
        [Required, MinLength(1), MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, Phone]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public Gender Gender { get; set; } = Gender.Male;
        [Required]
        public DateTime Birthdate { get; set; } = DateTime.Now.ToUniversalTime();
        [AllowNull]
        public string Address { get; set; } = string.Empty;
        [AllowNull]
        public string Citizenship { get; set; } = string.Empty;
    }
}
