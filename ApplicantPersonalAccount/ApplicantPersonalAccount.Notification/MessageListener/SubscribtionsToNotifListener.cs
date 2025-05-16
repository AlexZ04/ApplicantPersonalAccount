using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Notification.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ApplicantPersonalAccount.Notification.MessageListener
{
    public class SubscribtionsToNotifListener : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _config;

        private readonly string _queueName = "subscribtions_queue";

        public SubscribtionsToNotifListener(IServiceProvider serviceProvider,
            IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _config = config;

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

                var subDto = JsonSerializer.Deserialize<SubscriptionToNotificationDTO>(message);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    if (subDto!.Subscribe) 
                        await notificationService.SignUserToNotifications(subDto.UserEmail);
                    else
                        await notificationService.UnsignUserFromNotifications(subDto.UserEmail);
                }

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName,
                                  autoAck: false,
                                  consumer: consumer);
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
