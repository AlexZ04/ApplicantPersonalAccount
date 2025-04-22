using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.Common.Models.Document
{
    public class DocumentModel
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public DateTime UploadTime { get; set; }
        public FileDocumentType DocumentType { get; set; }
    }
}
