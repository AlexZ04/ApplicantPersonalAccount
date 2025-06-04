using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.Common.DTOs.Managers
{
    public class ManagerUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
    }
}
