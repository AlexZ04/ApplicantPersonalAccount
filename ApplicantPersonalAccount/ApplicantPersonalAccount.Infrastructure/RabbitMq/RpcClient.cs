using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Exceptions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ApplicantPersonalAccount.Infrastructure.RabbitMq
{
    public class RpcClient : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _replyQueue;
        private readonly EventingBasicConsumer _consumer;

        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _messagesData
            = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

        public RpcClient()
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = "localhost",
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _replyQueue = _channel
                          .QueueDeclare(queue: "",
                                        durable: false,
                                        exclusive: true,
                                        autoDelete: true,
                                        arguments: null).QueueName;

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += ProcessResponse;

            _channel.BasicConsume(queue: _replyQueue, autoAck: true, consumer: _consumer);
        }

        private void ProcessResponse(object? sender, BasicDeliverEventArgs eventArgs)
        {
            if (!_messagesData.TryRemove(eventArgs.BasicProperties.CorrelationId, out var taskSourse))
                return;

            var body = eventArgs.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);
            taskSourse.SetResult(response);

            //_channel.QueueDelete(_replyQueue);
        }

        public Task<string> CallAsync(object message, string requestQueue)
        {
            var corId = Guid.NewGuid();
            var props = _channel.CreateBasicProperties();

            props.CorrelationId = corId.ToString();
            props.ReplyTo = _replyQueue;

            var messageJson = JsonSerializer.Serialize(message);
            var messageBytes = Encoding.UTF8.GetBytes(messageJson);

            var taskSource = new TaskCompletionSource<string>();
            _messagesData.TryAdd(corId.ToString(), taskSource);

            _channel.BasicPublish(
                exchange: "",
                routingKey: requestQueue,
                basicProperties: props,
                body: messageBytes);

            var cancToken = new CancellationTokenSource(TimeSpan.FromSeconds(GeneralSettings.RPC_TIMEOUT));
            cancToken.Token.Register(() =>
            {
                if (_messagesData.TryRemove(corId.ToString(), out var taskSourse))
                {
                    taskSourse.TrySetException(new ProcessingException());
                }
            });

            return taskSource.Task;
        }
        
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
