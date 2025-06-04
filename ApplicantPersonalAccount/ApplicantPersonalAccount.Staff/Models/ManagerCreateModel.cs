using ApplicantPersonalAccount.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Staff.Models
{
    public class ManagerCreateModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; } = String.Empty;
        [Display(Name = "Role")]
        public Role Role { get; set; } = Role.Manager;
        [EmailAddress]
        [Display(Name = "Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Phone")]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = String.Empty;
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; } = DateTime.UtcNow.ToUniversalTime();
        [Display(Name = "Gender")]
        public Gender Gender { get; set; } = Gender.Male;
        [Display(Name = "Password")]
        [UIHint("password")]
        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
