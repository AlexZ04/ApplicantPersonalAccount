using RabbitMQ.Client;

namespace ApplicantPersonalAccount.Infrastructure.RabbitMq.Connection
{
    public interface IRabbitMqConnection
    {
        IConnection Connection { get; }
    }
}
