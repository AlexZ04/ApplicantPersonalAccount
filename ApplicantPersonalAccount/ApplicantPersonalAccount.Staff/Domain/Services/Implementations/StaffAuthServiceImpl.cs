using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;
using ApplicantPersonalAccount.Staff.Models;
using System.Text.Json;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Implementations
{
    public class StaffAuthServiceImpl : IStaffAuthService
    {
        private readonly IMessageProducer _messageProducer;

        public StaffAuthServiceImpl(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        public async Task Logout()
        {

        }

        public async Task<bool> Login(LoginViewModel loginModel)
        {
            var rpcClient = new RpcClient();
            var request = new UserLoginModel
            {
                Email = loginModel.UserEmail!,
                Password = loginModel.Password!
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.LOGIN);

            if (result == "")
                return false;

            var tokenData = JsonSerializer.Deserialize<TokenResponseModel>(result)!;

            return true;
        }
    }
}
