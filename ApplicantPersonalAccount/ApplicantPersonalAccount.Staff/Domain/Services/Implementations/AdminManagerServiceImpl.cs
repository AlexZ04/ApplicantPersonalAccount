using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Common.Models.User;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Infrastructure.Utilities;
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

        public AdminManagerServiceImpl(IMessageProducer messageProducer)
        {
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                Converters = { new JsonStringEnumConverter() }
            };

            _messageProducer = messageProducer;
        }

        public async Task<List<ManagerDTO>> GetListOfManagers()
        {
            var rpcClient = new RpcClient();
            var request = new BrokerRequestDTO
            {
                Request = "request"
            };

            var result = await rpcClient.CallAsync(request, RabbitQueues.GET_ALL_MANAGERS);
            if (result == null)
                return new List<ManagerDTO>();

            var userData = JsonSerializer.Deserialize<List<ManagerDTO>>(result, _jsonOptions)!;

            return userData;
        }

        public async Task<ManagerProfileViewModel> GetManagerProfile(Guid id)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = id
            };

            var result = await rpcClient.CallAsync(request, RabbitQueues.GET_USER_BY_ID);
            if (result == null)
                throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);

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
            var request = new GuidRequestDTO
            {
                Id = id
            };
            _messageProducer.SendMessage(request, RabbitQueues.DELETE_MANAGER);
        }

        public void EditManagerProfile(ManagerProfileViewModel model)
        {
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

            var result = await rpcClient.CallAsync(request, RabbitQueues.CREATE_MANGER);
            if (result == string.Empty)
                return false;

            return true;
        }
    }
}
