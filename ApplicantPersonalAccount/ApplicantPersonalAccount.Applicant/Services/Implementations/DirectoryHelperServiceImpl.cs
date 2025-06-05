using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using System.Text.Json;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class DirectoryHelperServiceImpl : IDirectoryHelperService
    {
        private readonly ILogger<DirectoryHelperServiceImpl> _logger;

        public DirectoryHelperServiceImpl(ILogger<DirectoryHelperServiceImpl> logger)
        {
            _logger = logger;
        }

        public async Task<List<DocumentType>> GetDocumentTypes()
        {
            _logger.LogInformation("Sending request to get document types");

            var rpcClient = new RpcClient();
            var request = new BrokerRequestDTO
            {
                Request = "request"
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_DOCUMENT_TYPE);
            if (result == null || result == "null")
            {
                _logger.LogWarning("Response with document types did not come");
                return new List<DocumentType>();
            }

            var documentTypes = JsonSerializer.Deserialize<List<DocumentType>>(result)!;

            return documentTypes;
        }

        public async Task<ProgramPagedList> GetListOfPrograms(
            string faculty,
            string educationForm,
            string language,
            string code,
            string name,
            int page = 1,
            int size = 5)
        {
            _logger.LogInformation("Sending request to get list of programs");

            var rpcClient = new RpcClient();
            var request = new GetProgramsDTO
            {
                Faculty = faculty,
                EducationForm = educationForm,
                Language = language,
                Code = code,
                Name = name,
                Page = page,
                Size = size
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_DIRECTORY_PROGRAMS);
            if (result == "")
            {
                _logger.LogWarning("Response with list of programs did not come");
                return new ProgramPagedList();
            }

            var programs = JsonSerializer.Deserialize<ProgramPagedList>(result)!;

            return programs;
        }
    }
}
