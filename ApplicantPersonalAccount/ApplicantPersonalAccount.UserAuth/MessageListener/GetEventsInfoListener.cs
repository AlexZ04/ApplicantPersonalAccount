using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Persistence.Repositories;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class GetEventsInfoListener : BaseMessageListener<GetInfoForEventsRequestDTO>
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public GetEventsInfoListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_INFO_FOR_EVENTS) 
        {
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        protected override async Task<string?> ProcessMessage(
            GetInfoForEventsRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            try
            {
                var userRep = serviceProvider.GetRequiredService<IUserRepository>();
                var userData = await userRep.GetInfoForEvents(message.UserId);
                return JsonSerializer.Serialize(userData, _jsonOptions);
            }
            catch
            {
                return "";
            }
        }
    }
}
