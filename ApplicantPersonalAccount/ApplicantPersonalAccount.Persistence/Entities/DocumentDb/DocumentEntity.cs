using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.Persistence.Entities.DocumentDb
{
    public class DocumentEntity
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public DateTime UploadTime { get; set; }
        public FileDocumentType DocumentType { get; set; }
        public Guid OwnerId { get; set; }
    }
}
