using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.Common.DTOs.Managers
{
    public class ManagerDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
