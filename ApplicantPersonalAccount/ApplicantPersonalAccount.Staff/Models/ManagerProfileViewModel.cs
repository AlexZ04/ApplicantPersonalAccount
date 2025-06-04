using ApplicantPersonalAccount.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Staff.Models
{
    public class ManagerProfileViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = String.Empty;
        [Display(Name = "Role")]
        public string Role { get; set; } = "Manager";
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Phone")]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = String.Empty;
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Birthday { get; set; } = DateTime.Now.ToUniversalTime();
        [Display(Name = "Gender")]
        public Gender Gender { get; set; } = Gender.Male;

        [Display(Name = "Citizenship")]
        public string Citizenship { get; set; } = string.Empty;

        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;
    }
} 