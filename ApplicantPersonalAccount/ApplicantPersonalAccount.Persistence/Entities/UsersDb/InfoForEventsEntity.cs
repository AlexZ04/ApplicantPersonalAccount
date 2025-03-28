namespace ApplicantPersonalAccount.Persistence.Entities.UsersDb
{
    public class InfoForEventsEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public string EducationPlace { get; set; } = string.Empty;
        public string SocialNetwork { get; set; } = string.Empty;
    }
}
