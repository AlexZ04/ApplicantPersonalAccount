using ApplicantPersonalAccount.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Staff.Models
{
    public class ManagerProfileViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Role")]
        public string Role { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Birthday")]
        public DateTime? Birthday { get; set; }
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Display(Name = "Citizenship")]
        public string Citizenship { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }
    }
} 