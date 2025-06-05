using ApplicantPersonalAccount.Applicant.Services;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.Applicant.MessageListeners
{
    public class CreateEnteranceListener : BaseMessageListener<GuidRequestDTO>
    {
        public CreateEnteranceListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.CREATE_ENTERANCE) { }

        protected override async Task<string?> ProcessMessage(
            GuidRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var enteranceService = serviceProvider.GetRequiredService<IEnteranceService>();

            await enteranceService.CreateEnteranceForUser(message.Id);

            return null;
        }
    }
}
