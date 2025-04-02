using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.Persistence.Entities.UsersDb
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Gender Gender { get; set; } = Gender.Male;
        public DateTime Birthdate { get; set; } = DateTime.Now.ToUniversalTime();
        public string Address { get; set; } = string.Empty;
        public string Citizenship { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Applicant;
        public DateTime CreateTime { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime UpdateTime { get; set; } = DateTime.Now.ToUniversalTime();
        public InfoForEventsEntity InfoForEvents { get; set; }
    }
}
