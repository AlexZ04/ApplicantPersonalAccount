using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.DirectoryService.MessageListener
{
    public class EducationProgramByIdListener : BaseMessageListener<GuidRequestDTO>
    {
        public EducationProgramByIdListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_EDUCATION_PROGRAM_BY_ID) { }

        protected override async Task<string?> ProcessMessage(
            GuidRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var directoryContext = serviceProvider.GetRequiredService<DirectoryDataContext>();

            var program = await directoryContext.EducationPrograms
                .Include(p => p.Faculty)
                .Include(p => p.EducationLevel)
                .FirstOrDefaultAsync(p => p.Id == message.Id);

            return JsonSerializer.Serialize(program);
        }
    }
}
