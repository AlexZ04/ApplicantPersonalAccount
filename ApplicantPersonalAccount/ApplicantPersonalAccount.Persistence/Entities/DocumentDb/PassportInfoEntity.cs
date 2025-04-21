namespace ApplicantPersonalAccount.Persistence.Entities.DocumentDb
{
    public class PassportInfoEntity
    {
        public Guid Id { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public string BirthPlace { get; set; }
        public string WhenIssued { get; set; }
        public string ByWhoIssued { get; set; }
        public Guid UserId { get; set; }
    }
}
