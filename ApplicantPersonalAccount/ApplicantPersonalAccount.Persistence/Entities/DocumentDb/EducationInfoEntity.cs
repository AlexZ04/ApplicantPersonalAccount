namespace ApplicantPersonalAccount.Persistence.Entities.DocumentDb
{
    public class EducationInfoEntity
    {
        public Guid Id { get; set; }
        public Guid? DocumentTypeId { get; set; }
        public Guid UserId { get; set; }
        public DocumentEntity Document { get; set; }
    }
}
