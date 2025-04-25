namespace ApplicantPersonalAccount.Infrastructure.RabbitMq
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message, string order);
    }
}
