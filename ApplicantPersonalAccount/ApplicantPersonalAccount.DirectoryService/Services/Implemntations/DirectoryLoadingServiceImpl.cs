using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Repositories;

namespace ApplicantPersonalAccount.DirectoryService.Services.Implemntations
{
    public class DirectoryLoadingServiceImpl : IDirectoryLoadingService
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IDirectoryService _directoryService;
        private readonly DirectoryDataContext _directoryContext;
        private readonly ILogger<DirectoryLoadingServiceImpl> _logger;

        public DirectoryLoadingServiceImpl(
            IDirectoryRepository directoryRepository,
            IDirectoryService directoryService,
            DirectoryDataContext directoryContext,
            ILogger<DirectoryLoadingServiceImpl> logger)
        {
            _directoryRepository = directoryRepository;
            _directoryService = directoryService;
            _directoryContext = directoryContext;
            _logger = logger;
        }

        public async Task<DocumentType> GetDocumentTypeById(Guid id)
        {
            var foundDocumentType = await _directoryContext.DocumentTypes
                .FindAsync(id);

            return foundDocumentType ?? throw new NotFoundException(ErrorMessages.DOCUMENT_TYPE_NOT_FOUND);
        }

        public LoadingStatusDTO GetLoadingStatus()
        {
            _logger.LogInformation($"Current import status is requested");

            return new LoadingStatusDTO
            {
                Status = ImportStatusHolder.ImportStatus,
            };
        }

        public async Task RequestDictImport(DirectoryImportType importType)
        {
            var importString = string.Format("Importing dictionary: {0}", importType);

            ImportStatusHolder.ImportStatus = importString;
            _logger.LogInformation(importString);

            switch (importType)
            {
                case (DirectoryImportType.DocumentTypes):
                    await LoadDocumentTypes();

                    break;

                case (DirectoryImportType.Faculties):
                    await LoadFaculties();

                    break;

                case (DirectoryImportType.EducationLevels):
                    await LoadEducationLevels();

                    break;

                case (DirectoryImportType.Programs):
                    await LoadPrograms();

                    break;

                case (DirectoryImportType.All):
                    await _directoryRepository.ResetAll();

                    await LoadEducationLevels();
                    await LoadDocumentTypes();
                    await LoadFaculties();
                    await LoadPrograms();

                    break;

                default:
                    break;
            }

            ImportStatusHolder.ImportStatus = ImportStatuses.ALL_LOADED;
        }

        private async Task LoadDocumentTypes()
        {
            var importString = string.Format("Receiving data: {0}", "document types");
            ImportStatusHolder.ImportStatus = importString;
            _logger.LogInformation(importString);
                
            var documentTypes = await _directoryService.GetDocumentTypes();

            await Task.Delay(1000);

            importString = string.Format("Setting data: {0}", "document types");
            ImportStatusHolder.ImportStatus = importString;
            _logger.LogInformation(importString);

            await _directoryRepository.SetDocumentTypes(documentTypes);

            await Task.Delay(1000);
        }

        private async Task LoadFaculties()
        {
            var importString = string.Format("Receiving data: {0}", "faculties");
            ImportStatusHolder.ImportStatus = importString;
            _logger.LogInformation(importString);

            var faculties = await _directoryService.GetFaculties();

            await Task.Delay(1000);

            importString = string.Format("Setting data: {0}", "faculties");
            ImportStatusHolder.ImportStatus = importString;
            _logger.LogInformation(importString);
                
            await _directoryRepository.SetFaculties(faculties);

            await Task.Delay(1000);
        }

        private async Task LoadEducationLevels()
        {
            var importString = string.Format("Receiving data: {0}", "education levels");
            ImportStatusHolder.ImportStatus = importString;
            _logger.LogInformation(importString);
                
            var educationLevels = await _directoryService.GetEducationLevels();

            await Task.Delay(1000);

            importString = string.Format("Setting data: {0}", "education levels");
            ImportStatusHolder.ImportStatus = importString;
            _logger.LogInformation(importString);
                
            await _directoryRepository.SetEducationLevels(educationLevels);

            await Task.Delay(1000);
        }

        private async Task LoadPrograms()
        {
            var programs = await _directoryService.GetEducationPrograms(1, 1);
            var size = programs.Pagination.Count;

            var importString = string.Format("Receiving data: {0}", "programs");
            ImportStatusHolder.ImportStatus = importString;
            _logger.LogInformation(importString);

            programs = await _directoryService.GetEducationPrograms(1, size);

            await Task.Delay(1000);

            importString = string.Format("Setting data: {0}", "programs");
            ImportStatusHolder.ImportStatus = importString;
            _logger.LogInformation(importString);

            await _directoryRepository.SetEducationPrograms(programs.Programs);

            await Task.Delay(1000);
        }
    }
}
