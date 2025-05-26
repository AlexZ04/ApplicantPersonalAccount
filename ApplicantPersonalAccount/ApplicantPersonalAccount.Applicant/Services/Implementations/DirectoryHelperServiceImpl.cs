using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using System.Text.Json;
using System.Web.Helpers;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class DirectoryHelperServiceImpl : IDirectoryHelperService
    {
        public async Task<List<DocumentType>> GetDocumentTypes()
        {
            var rpcClient = new RpcClient();
            var request = new BrokerRequestDTO
            {
                Request = "request"
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_DOCUMENT_TYPE);
            if (result == null) return new List<DocumentType>();

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
            if (result == "") return new ProgramPagedList();

            var programs = JsonSerializer.Deserialize<ProgramPagedList>(result)!;

            return programs;
        }
    }
}
