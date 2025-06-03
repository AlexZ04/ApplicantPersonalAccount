using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.User;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;
using ApplicantPersonalAccount.Staff.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Implementations
{
    public class AdminManagerServiceImpl : IAdminManagerService
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public AdminManagerServiceImpl()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        public async Task<List<ManagerDTO>> GetListOfManagers()
        {
            return new List<ManagerDTO>() 
            {
                new ManagerDTO
                {
                    Name = "Test Name",
                    Email = "abcaf@gmail.com",
                    Role = "Manager"
                },
                new ManagerDTO
                {
                    Name = "Test Name",
                    Email = "abcaf@gmail.com",
                    Role = "Main Manager"
                },
            };
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
    }
}
