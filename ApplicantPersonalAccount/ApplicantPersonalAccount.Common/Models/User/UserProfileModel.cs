using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.Common.Models.User
{
    public class UserProfileModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Applicant;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Gender Gender { get; set; } = Gender.Male;
        public DateTime Birthdate { get; set; } = DateTime.Now.ToUniversalTime();
        public string Citizenship { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
