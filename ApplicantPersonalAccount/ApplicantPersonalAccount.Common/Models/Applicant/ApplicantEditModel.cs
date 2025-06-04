using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApplicantPersonalAccount.Common.Models.Applicant
{
    public class ApplicantEditModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        [AllowNull]
        public DateTime Birthdate { get; set; } = DateTime.Now.ToUniversalTime();
        [AllowNull]
        public string Citizenship { get; set; } = String.Empty;
        [AllowNull]
        public string Address { get; set; } = String.Empty;
    }
}
