using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.Document;
using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;

namespace ApplicantPersonalAccount.Persistence.Repositories
{
    public interface IDocumentRepository
    {
        public Task AddFile(DocumentEntity file);
        public Task DeleteDocumentById(Guid id);
        public Task<DocumentEntity> GetDocumentInfoById(Guid id);
        public Task<List<DocumentEntity>> GetUserDocuments(FileDocumentType documentType, 
            Guid userId, 
            bool importingAll = false);
    }
}
