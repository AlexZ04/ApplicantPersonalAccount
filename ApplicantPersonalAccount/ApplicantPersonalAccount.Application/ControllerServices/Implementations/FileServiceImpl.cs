using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Document;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ApplicantPersonalAccount.Application.ControllerServices.Implementations
{
    public class FileServiceImpl : IFileService
    {
        private readonly string _pathToStorage;
        private readonly IDocumentRepository _documentRepository;

        public FileServiceImpl(IDocumentRepository documentRepository)
        {
            _pathToStorage = Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");

            if (!Directory.Exists(_pathToStorage))
                Directory.CreateDirectory(_pathToStorage);

            _documentRepository = documentRepository;
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

        public async Task EditPassport(PassportInfoEditModel editedPassport, Guid userId)
        {
            await _documentRepository.EditPassport(editedPassport, userId);
        }
    }
}
