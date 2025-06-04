using ApplicantPersonalAccount.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Common.DTOs.Managers
{
    public class ManagerCreateDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = String.Empty;
        public Role Role { get; set; } = Role.Manager;
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string Phone { get; set; } = String.Empty;
        public DateTime? Birthday { get; set; } = DateTime.UtcNow;
        public Gender Gender { get; set; } = Gender.Male;
        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
