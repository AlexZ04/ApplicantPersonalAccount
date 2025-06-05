using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;
using ApplicantPersonalAccount.Staff.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Implementations
{
    public class AdminManagerServiceImpl : IAdminManagerService
    {
        private readonly IMessageProducer _messageProducer;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<AdminDirectoryServiceImpl> _logger;

        public AdminManagerServiceImpl(
            IMessageProducer messageProducer,
            ILogger<AdminDirectoryServiceImpl> logger)
        {
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                Converters = { new JsonStringEnumConverter() }
            };

            _messageProducer = messageProducer;
            _logger = logger;
        }

        public async Task<List<ManagerDTO>> GetListOfManagers()
        {
            _logger.LogInformation("Sending request to get list of managers");

            var rpcClient = new RpcClient();
            var request = new BrokerRequestDTO
            {
                Request = "request"
            };

            var result = await rpcClient.CallAsync(request, RabbitQueues.GET_ALL_MANAGERS);
            if (result == null || result == "null")
            {
                _logger.LogError("Response with list of managers did not come");
                return new List<ManagerDTO>();
            }
                
            var userData = JsonSerializer.Deserialize<List<ManagerDTO>>(result, _jsonOptions)!;

            return userData;
        }

        public async Task<ManagerProfileViewModel> GetManagerProfile(Guid id)
        {
            _logger.LogInformation($"Sending request to get user profile. Id: {id}");

            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = id
            };

            var result = await rpcClient.CallAsync(request, RabbitQueues.GET_USER_BY_ID);
            if (result == null || result == "null")
            {
                _logger.LogError($"User with id {id} not found");
                throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);
            }
            
            var userData = JsonSerializer.Deserialize<ManagerProfileDTO>(result, _jsonOptions)!;

            var managerModel = new ManagerProfileViewModel
            {
                Id = id,
                Name = userData.Name,
                Email = userData.Email,
                Phone = userData.Phone,
                Address = userData.Address,
                Citizenship = userData.Citizenship,
                Birthday = userData.Birthdate,
                Gender = userData.Gender,
                Role = userData.Role == Role.Manager ? "Manager" : "Main Manager"
            };

            return managerModel;
        }

        public void DeleteManager(Guid id)
        {
            _logger.LogInformation($"Sending request to delete user with {id}");

            var request = new GuidRequestDTO
            {
                Id = id
            };
            _messageProducer.SendMessage(request, RabbitQueues.DELETE_MANAGER);
        }

        public void EditManagerProfile(ManagerProfileViewModel model)
        {
            _logger.LogInformation($"Sending request to update user with {model.Id}");

            var request = new ManagerUpdateDTO
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Birthday = model.Birthday,
                Gender = model.Gender
            };

            _messageProducer.SendMessage(request, RabbitQueues.UPDATE_MANAGER);
        }

        public async Task<bool> CreateManager(ManagerCreateModel model)
        {
            _logger.LogInformation($"Sending request create manager {model.Email}");

            var rpcClient = new RpcClient();

            var request = new ManagerCreateDTO
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Birthday = model.Birthday,
                Gender = model.Gender,
                Role = model.Role,
                Password = model.Password
            };

            var result = await rpcClient.CallAsync(request, RabbitQueues.CREATE_MANAGER);
            if (result == string.Empty)
            {
                _logger.LogError($"Something went wrong with creating manager {model.Email}");
                return false;
            }

            return true;
        }
    }
}
