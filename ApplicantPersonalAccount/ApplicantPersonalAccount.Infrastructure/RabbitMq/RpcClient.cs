using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace ApplicantPersonalAccount.Infrastructure.RabbitMq
{
    public class RpcClient : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RpcClient(string replyQueue)
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = "localhost",
            }
            ;
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
