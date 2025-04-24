namespace ApplicantPersonalAccount.Persistence.Entities.ApplicationDb
{
    public class SignedToNotificationsEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime SigningTime { get; set; }
    }
}
