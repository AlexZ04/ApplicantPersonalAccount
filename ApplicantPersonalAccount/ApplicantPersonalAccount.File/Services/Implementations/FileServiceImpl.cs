using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Common.Models.Document;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ApplicantPersonalAccount.Document.Services.Implementations
{
    public class FileServiceImpl : IFileService
    {
        private readonly string _pathToStorage;
        private readonly FileDataContext _fileContext;
        private readonly IDocumentRepository _documentRepository;

        public FileServiceImpl(
            IDocumentRepository documentRepository,
            FileDataContext fileContext)
        {
            _pathToStorage = Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");

            if (!Directory.Exists(_pathToStorage))
                Directory.CreateDirectory(_pathToStorage);

            _documentRepository = documentRepository;
            _fileContext = fileContext;
        }

        public async Task UploadFile(FileDocumentType documentType, IFormFile file, Guid userId)
        {
            var fileName = Hasher.HashFilename(file.FileName) + Path.GetExtension(file.FileName);
            var pathToNewFile = Path.Combine(_pathToStorage, fileName);

            using (var stream = new FileStream(pathToNewFile, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var newDocument = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Filename = fileName,
                Path = pathToNewFile,
                UploadTime = DateTime.Now.ToUniversalTime(),
                DocumentType = documentType,
                OwnerId = userId
            };

            await _documentRepository.AddFile(newDocument);
        }

        public async Task DeleteFile(Guid id, Guid userId)
        {
            var document = await _documentRepository.GetDocumentInfoById(id);

            if (document.OwnerId != userId)
                throw new UnaccessableAction(ErrorMessages.CANT_DELETE_THIS_FILE);

            var pathToFile = Path.Combine(_pathToStorage, document.Filename);

            if (File.Exists(pathToFile))
                File.Delete(pathToFile);

            await _documentRepository.DeleteDocumentById(id);
        }

        public async Task<DocumentModel> GetDocumentInfo(Guid id)
        {
            var document = await _documentRepository.GetDocumentInfoById(id);

            return new DocumentModel
            {
                Id = document.Id,
                Filename = document.Filename,
                UploadTime = document.UploadTime,
                DocumentType = document.DocumentType
            };
        }

        public async Task<FileContentResult> GetFile(Guid id)
        {
            var document = await _documentRepository.GetDocumentInfoById(id);

            if (!File.Exists(document.Path))
                throw new NotFoundException(ErrorMessages.FILE_IS_NOT_ON_DISK);

            var fileBytes = await File.ReadAllBytesAsync(document.Path);

            return new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = document.Filename
            };
        }

        public async Task<List<DocumentModel>> GetUserDocuments(FileDocumentType documentType,
            Guid userId,
            bool importingAll = false)
        {
            var documents = await _documentRepository.GetUserDocuments(documentType, userId, importingAll);

            var userDocuments = new List<DocumentModel>();
            foreach (var document in documents)
            {
                userDocuments.Add(new DocumentModel
                {
                    Id = document.Id,
                    Filename = document.Filename,
                    UploadTime = document.UploadTime,
                    DocumentType = document.DocumentType
                });
            }

            return userDocuments;
        }

        public async Task EditPassport(PassportInfoEditModel editedPassport, Guid userId, string userRole)
        {
            if (userRole == "Applicant")
                await CheckEditable(userId);

            await _documentRepository.EditPassport(editedPassport, userId);
        }

        public async Task EditEducational(
            EducationInfoEditModel editedEducation,
            Guid documentId,
            Guid userId,
            string userRole)
        {
            if (userRole == "Applicant")
                await CheckEditable(userId);

            await _documentRepository.EditEducational(editedEducation, documentId, userId);
        }

        public async Task<DocumentType> GetEducationDocumentInfo(Guid documentId)
        {
            var info = await _fileContext.EducationInfos
                .Include(i => i.Document)
                .FirstOrDefaultAsync(i => i.Document.Id == documentId);

            if (info == null)
                throw new NotFoundException(ErrorMessages.DOCUMENT_NOT_FOUND);

            if (info.DocumentTypeId == null)
                throw new NotFoundException(ErrorMessages.THERE_IS_NO_INFO_FOR_THIS_FILE);

            var documentType = await GetDocumentTypeById((Guid)info.DocumentTypeId);

            return documentType;
        }

        public async Task<PassportInfoModel> GetPassportInfo(Guid userId)
        {
            var info = await _fileContext.PassportInfos
                .FirstOrDefaultAsync(i => i.UserId == userId);

            if (info == null)
                throw new NotFoundException(ErrorMessages.DOCUMENT_NOT_FOUND);

            return new PassportInfoModel
            {
                Series = info.Series,
                Number = info.Number,
                BirthPlace = info.BirthPlace,
                WhenIssued = info.WhenIssued,
                ByWhoIssued = info.ByWhoIssued
            };
        }

        private async Task CheckEditable(Guid userId)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = userId
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.CAN_EDIT_LISTENER);
            if (result == null || result == "false")
                throw new InvalidActionException(ErrorMessages.USER_CANT_EDIT_THIS_DATA);
        }

        private async Task<DocumentType> GetDocumentTypeById(Guid id)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = id
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_DOCUMENT_TYPE_BY_ID);
            if (result == null) throw new NotFoundException(ErrorMessages.DOCUMENT_TYPE_NOT_FOUND);

            var documentType = JsonSerializer.Deserialize<DocumentType>(result)!;

            return documentType;
        }
    }
}
