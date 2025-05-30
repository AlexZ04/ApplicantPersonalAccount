using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;
using ApplicantPersonalAccount.Staff.Models;

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
            return true;
        }
    }
}
