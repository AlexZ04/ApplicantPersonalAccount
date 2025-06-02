using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Common.MVC;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;
using ApplicantPersonalAccount.Common.Enums;
using System.Text.Json;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Implementations
{
    public class AdminDirectoryServiceImpl : IAdminDirectoryService
    {
        private readonly IMessageProducer _messageProducer;

        public AdminDirectoryServiceImpl(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

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

        public void RequestImport(DirectoryImportType importType)
        {
            var request = new ImportRequestDTO
            {
                Type = importType
            };

            _messageProducer.SendMessage(request, RabbitQueues.IMPORT_REQUEST);
        }
    }
}
