using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
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

        public async Task UploadFile(FileDocumentType documentType, IFormFile file, ClaimsPrincipal user)
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
                OwnerId = UserDescriptor.GetUserId(user)
            };

            await _documentRepository.AddFile(newDocument);
        }
    }
}
