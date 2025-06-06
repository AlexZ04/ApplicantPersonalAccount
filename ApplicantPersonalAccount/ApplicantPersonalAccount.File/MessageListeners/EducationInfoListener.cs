using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Document.Services;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Persistence.Repositories;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.Document.MessageListeners
{
    public class EducationInfoListener : BaseMessageListener<GuidRequestDTO>
    {
        public EducationInfoListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_EDUCATION_INFO) { }

        protected override async Task<string?> ProcessMessage(
            GuidRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var directoryInfoService = serviceProvider.GetRequiredService<IDocumentRepository>();

            try
            {
                var data = await directoryInfoService.GetEducationInfoById(message.Id);
                return JsonSerializer.Serialize(data);
            }
            catch
            {
                return "";
            }

        }
    }
}
