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
        private readonly ILogger<AdminDirectoryServiceImpl> _logger;

        public AdminDirectoryServiceImpl(
            IMessageProducer messageProducer,
            ILogger<AdminDirectoryServiceImpl> logger)
        {
            _messageProducer = messageProducer;
            _logger = logger;
        }

        public async Task<string> GetImportStatus()
        {
            _logger.LogInformation("Send request to get import status");

            var rpcClient = new RpcClient();
            var request = new BrokerRequestDTO
            {
                Request = "request"
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.IMPORT_STATUS);
            rpcClient.Dispose();
            if (result == "")
            {
                _logger.LogError("Error with getting import status");
                return PageElementsNames.ERROR_WITH_STATUS;
            }
                
            return result;
        }

        public void RequestImport(DirectoryImportType importType)
        {
            var request = new ImportRequestDTO
            {
                Type = importType
            };

            _logger.LogInformation($"Sending request to import directory. Import type: {importType}");

            _messageProducer.SendMessage(request, RabbitQueues.IMPORT_REQUEST);
        }
    }
}
