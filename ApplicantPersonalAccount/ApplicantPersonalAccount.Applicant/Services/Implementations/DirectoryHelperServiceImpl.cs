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
            throw new NotImplementedException();
        }
    }
}
