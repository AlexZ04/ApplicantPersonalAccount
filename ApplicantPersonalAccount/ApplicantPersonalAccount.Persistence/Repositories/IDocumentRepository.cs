using ApplicantPersonalAccount.Common.Models.Document;
using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;

namespace ApplicantPersonalAccount.Persistence.Repositories
{
    public interface IDocumentRepository
    {
        public Task AddFile(DocumentEntity file);
        public Task<DocumentEntity> GetDocumentInfoById(Guid id);
    }
}
