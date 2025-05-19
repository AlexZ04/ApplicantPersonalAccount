using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;

namespace ApplicantPersonalAccount.Application.ControllerServices.Implementations
{
    public class StaffServiceImpl : IStaffService
    {
        private readonly IMessageProducer _messageProducer;

        public StaffServiceImpl(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        public async Task Save()
        {
            _messageProducer.SendMessage("{testMessage:\"a\"}", "testOrder");
        }
    }
}
