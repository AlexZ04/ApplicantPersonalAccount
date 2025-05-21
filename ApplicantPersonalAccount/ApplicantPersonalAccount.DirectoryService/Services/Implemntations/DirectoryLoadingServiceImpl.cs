using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace ApplicantPersonalAccount.DirectoryService.Services.Implemntations
{
    public class DirectoryLoadingServiceImpl : IDirectoryLoadingService
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IDirectoryService _directoryService;
        private readonly DirectoryDataContext _directoryContext;

        public DirectoryLoadingServiceImpl(
            IDirectoryRepository directoryRepository,
            IDirectoryService directoryService,
            DirectoryDataContext directoryContext)
        {
            _directoryRepository = directoryRepository;
            _directoryService = directoryService;
            _directoryContext = directoryContext;
        }

        public async Task<DocumentType> GetDocumentTypeById(Guid id)
        {
            var foundDocumentType = await _directoryContext.DocumentTypes
                .FindAsync(id);

            return foundDocumentType ?? throw new NotFoundException(ErrorMessages.DOCUMENT_TYPE_NOT_FOUND);
        }

        public LoadingStatusDTO GetLoadingStatus()
        {
            return new LoadingStatusDTO
            {
                Status = ImportStatusHolder.ImportStatus,
            };
        }

        public async Task RequestDictImport(DirectoryImportType importType)
        {
            ImportStatusHolder.ImportStatus = string.Format("Importing dictionary: {0}", importType);

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
            ImportStatusHolder.ImportStatus =
                string.Format("Receiving data: {0}", "document types");
            var documentTypes = await _directoryService.GetDocumentTypes();

            ImportStatusHolder.ImportStatus =
                string.Format("Setting data: {0}", "document types");
            await _directoryRepository.SetDocumentTypes(documentTypes);
        }

        private async Task LoadFaculties()
        {
            ImportStatusHolder.ImportStatus =
                string.Format("Receiving data: {0}", "faculties");
            var faculties = await _directoryService.GetFaculties();

            ImportStatusHolder.ImportStatus =
                string.Format("Setting data: {0}", "faculties");
            await _directoryRepository.SetFaculties(faculties);
        }

        private async Task LoadEducationLevels()
        {
            ImportStatusHolder.ImportStatus =
                string.Format("Receiving data: {0}", "education levels");
            var educationLevels = await _directoryService.GetEducationLevels();

            ImportStatusHolder.ImportStatus =
                string.Format("Setting data: {0}", "education levels");
            await _directoryRepository.SetEducationLevels(educationLevels);
        }

        private async Task LoadPrograms()
        {
            var programs = await _directoryService.GetEducationPrograms(1, 1);
            var size = programs.Pagination.Count;

            ImportStatusHolder.ImportStatus =
                string.Format("Receiving data: {0}", "programs");
            programs = await _directoryService.GetEducationPrograms(1, size);

            ImportStatusHolder.ImportStatus =
                string.Format("Setting data: {0}", "programs");
            await _directoryRepository.SetEducationPrograms(programs.Programs);
        }
    }
}
