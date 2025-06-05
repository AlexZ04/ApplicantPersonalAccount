using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener
{
    public abstract class BaseMessageListener<TMessage> : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _config;

        private readonly string _queueName;

        public BaseMessageListener(
            IServiceProvider serviceProvider,
            IConfiguration config, 
            string queueName)
        {
            _serviceProvider = serviceProvider;
            _config = config;

            _queueName = queueName;

            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:Host"],
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var deserialized = JsonSerializer.Deserialize<TMessage>(message);

                using var scope = _serviceProvider.CreateScope();
                var res = await ProcessMessage(deserialized!, eventArgs, scope.ServiceProvider);

                if (res != null &&
                    (eventArgs.BasicProperties.ReplyTo != null && eventArgs.BasicProperties.ReplyTo != ""))
                {
                    var replyProps = _channel.CreateBasicProperties();
                    replyProps.CorrelationId = eventArgs.BasicProperties.CorrelationId;

                    var responseBytes = Encoding.UTF8.GetBytes(res ?? "");

                    _channel.BasicPublish(
                        exchange: "",
                        routingKey: eventArgs.BasicProperties.ReplyTo,
                        basicProperties: replyProps,
                        body: responseBytes);
                }

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName,
                                  autoAck: false,
                                  consumer: consumer);
        }

        protected abstract Task<string?> ProcessMessage(
            TMessage message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider);

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
