using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Common.MVC;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Implementations
{
    public class AdminDirectoryServiceImpl : IAdminDirectoryService
    {
        public async Task<string> GetImportStatus()
        {
            var rpcClient = new RpcClient();
            var request = new BrokerRequestDTO
            {
                Request = "request"
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.IMPORT_STATUS);

            if (result == "")
                return PageElementsNames.ERROR_WITH_STATUS;

            return result;
        }
    }
}
