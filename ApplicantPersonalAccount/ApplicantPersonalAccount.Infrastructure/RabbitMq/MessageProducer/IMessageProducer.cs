namespace ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message, string order);
    }
}
