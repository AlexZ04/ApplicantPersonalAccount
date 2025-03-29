namespace ApplicantPersonalAccount.Persistence.Entities.UsersDb
{
    public class RefreshTokenEntity
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public UserEntity User { get; set; }
        public DateTime Expires { get; set; }
    }
}
