using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Document;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Persistence.Repositories.Implementations
{
    public class DocumentRepositoryImpl : IDocumentRepository
    {
        private readonly FileDataContext _fileDataContext;

        public DocumentRepositoryImpl(FileDataContext fileDataContext)
        {
            _fileDataContext = fileDataContext;
        }

        public async Task AddFile(DocumentEntity file)
        {
            _fileDataContext.Documents.Add(file);
            await _fileDataContext.SaveChangesAsync();
        }

        public async Task DeleteDocumentById(Guid id)
        {
            var file = await _fileDataContext.Documents.FindAsync(id);

            if (file == null)
                throw new NotFoundException(ErrorMessages.FILE_NOT_FOUND);

            _fileDataContext.Documents.Remove(file);
            await _fileDataContext.SaveChangesAsync();
        }

        public async Task<DocumentEntity> GetDocumentInfoById(Guid id)
        {
            var file = await _fileDataContext.Documents.FindAsync(id);

            return file ?? throw new NotFoundException(ErrorMessages.FILE_NOT_FOUND);
        }

        public async Task<List<DocumentEntity>> GetUserDocuments(FileDocumentType documentType, Guid userId)
        {
            var documents = await _fileDataContext.Documents
                .Where(d => d.DocumentType == documentType && d.OwnerId == userId)
                .ToListAsync();

            return documents;
        }
    }
}
