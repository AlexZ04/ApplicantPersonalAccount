using ApplicantPersonalAccount.Applicant.Services;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Persistence.Repositories;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.Applicant.MessageListeners
{
    public class CanEditListener : BaseMessageListener<GuidRequestDTO>
    {
        public CanEditListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.CAN_EDIT_LISTENER) { }

        protected override async Task<string?> ProcessMessage(
            GuidRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var applicantService = serviceProvider.GetRequiredService<IApplicantService>();

            var canEdit = await applicantService.CanUserEdit(message.Id);

            return JsonSerializer.Serialize(canEdit);
        }
    }
}
