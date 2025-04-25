using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace ApplicantPersonalAccount.Infrastructure.RabbitMq.Connection
{
    public class RabbitMqConnection : IRabbitMqConnection, IDisposable
    {
        private IConnection? _connection;
        private readonly IConfiguration _config;

        public IConnection Connection => _connection!;

        public RabbitMqConnection(IConfiguration config)
        {
            _config = config;
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:Host"],
            };

            _connection = factory.CreateConnection();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
