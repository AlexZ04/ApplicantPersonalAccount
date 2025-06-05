using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Document;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ApplicantPersonalAccount.Persistence.Repositories.Implementations
{
    public class DocumentRepositoryImpl : IDocumentRepository
    {
        private readonly FileDataContext _fileDataContext;
        private readonly ILogger<DocumentRepositoryImpl> _logger;

        public DocumentRepositoryImpl(
            FileDataContext fileDataContext,
            ILogger<DocumentRepositoryImpl> logger)
        {
            _fileDataContext = fileDataContext;
            _logger = logger;
        }

        public async Task AddFile(DocumentEntity file)
        {
            _fileDataContext.Documents.Add(file);

            if (file.DocumentType == FileDocumentType.Passport &&
                await _fileDataContext.PassportInfos.FirstOrDefaultAsync(i => i.UserId == file.OwnerId) == null)
            {
                var newPassportField = new PassportInfoEntity
                {
                    Id = Guid.NewGuid(),
                    Series = string.Empty,
                    Number = string.Empty,
                    BirthPlace = string.Empty,
                    WhenIssued = string.Empty,
                    ByWhoIssued = string.Empty,
                    UserId = file.OwnerId
                };


                _fileDataContext.PassportInfos.Add(newPassportField);
            }

            if (file.DocumentType == FileDocumentType.Educational)
            {
                var newEducationInfo = new EducationInfoEntity
                {
                    Id = Guid.NewGuid(),
                    DocumentTypeId = null,
                    UserId= file.OwnerId,
                    Document = file
                };

                _fileDataContext.EducationInfos.Add(newEducationInfo);
            }

            await _fileDataContext.SaveChangesAsync();
        }

        public async Task DeleteDocumentById(Guid id)
        {
            var file = await _fileDataContext.Documents.FindAsync(id);

            if (file == null)
            {
                _logger.LogWarning($"Document {id} not found");
                throw new NotFoundException(ErrorMessages.FILE_NOT_FOUND);
            }

            if (file.DocumentType == FileDocumentType.Educational)
            {
                var educationInfo = await _fileDataContext.EducationInfos
                    .Include(i => i.Document)
                    .FirstAsync(i => i.Document == file);

                _fileDataContext.EducationInfos.Remove(educationInfo);
            }
            else
            {
                var anotherPassportInfo = await _fileDataContext.Documents
                    .Where(d => d.OwnerId == file.OwnerId 
                    && d.DocumentType == FileDocumentType.Passport)
                    .CountAsync();

                if (anotherPassportInfo <= 1)
                {
                    var passportInfo = await _fileDataContext.PassportInfos
                        .FirstAsync(p => p.UserId == file.OwnerId);

                    _fileDataContext.PassportInfos.Remove(passportInfo);
                }
            }

            _fileDataContext.Documents.Remove(file);
            await _fileDataContext.SaveChangesAsync();
        }

        public async Task<DocumentEntity> GetDocumentInfoById(Guid id)
        {
            var file = await _fileDataContext.Documents.FindAsync(id);

            if (file == null)
                _logger.LogWarning($"File {id} not found");

            return file ?? throw new NotFoundException(ErrorMessages.FILE_NOT_FOUND);
        }

        public async Task<List<DocumentEntity>> GetUserDocuments(FileDocumentType documentType,
            Guid userId,
            bool importingAll = false)
        {
            var documents = await _fileDataContext.Documents
                .Where(d => (d.DocumentType == documentType || importingAll) && d.OwnerId == userId)
                .ToListAsync();

            return documents;
        }

        public async Task EditPassport(PassportInfoEditModel editedPassport, Guid userId)
        {
            var passport = await _fileDataContext.PassportInfos
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (passport == null)
            {
                _logger.LogWarning($"User {userId} not found");
                throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);
            }

            passport.Series = editedPassport.Series;
            passport.Number = editedPassport.Number;
            passport.BirthPlace = editedPassport.BirthPlace;
            passport.WhenIssued = editedPassport.WhenIssued;
            passport.ByWhoIssued = editedPassport.ByWhoIssued;

            await _fileDataContext.SaveChangesAsync();
        }

        public async Task EditEducational(EducationInfoEditModel editedEducation, 
            Guid documentId, 
            Guid userId)
        {
            var education = await _fileDataContext.EducationInfos
                .Include(i => i.Document)
                .FirstOrDefaultAsync(i => i.UserId == userId && i.Document.Id == documentId);

            if (education == null)
            {
                _logger.LogWarning($"User {userId} not found");
                throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);
            }

            if (editedEducation.DocumentTypeId != null)
                await GetDocumentTypeById((Guid)editedEducation.DocumentTypeId);

            education.DocumentTypeId = editedEducation.DocumentTypeId;

            await _fileDataContext.SaveChangesAsync();
        }

        public async Task<DocumentType> GetDocumentTypeById(Guid id)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = id
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_DOCUMENT_TYPE_BY_ID);
            if (result == null)
            {
                _logger.LogWarning($"Document type {id} not found");
                throw new NotFoundException(ErrorMessages.DOCUMENT_TYPE_NOT_FOUND);
            }
                
            var documentType = JsonSerializer.Deserialize<DocumentType>(result)!;

            return documentType;
        }
    }
}
