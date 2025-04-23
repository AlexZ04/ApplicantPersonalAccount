namespace ApplicantPersonalAccount.Persistence.Entities.DocumentDb
{
    public class EducationInfoEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid UserId { get; set; }
        public DocumentEntity Document { get; set; }
    }
}
