using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Implementations
{
    public class StaffAuthServiceImpl : IStaffAuthService
    {
        private readonly IMessageProducer _messageProducer;

        public StaffAuthServiceImpl(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }
    }
}
