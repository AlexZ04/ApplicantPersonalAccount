using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.DirectoryService.Services;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.DirectoryService.MessageListener
{
    public class ProgramListener : BaseMessageListener<GetProgramsDTO>
    {
        public ProgramListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_DIRECTORY_PROGRAMS) { }

        protected override async Task<string?> ProcessMessage(
            GetProgramsDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var directoryInfoService = serviceProvider.GetRequiredService<IDirectoryInfoService>();

            try
            {
                var data = await directoryInfoService.GetListOfPrograms(message);
                return JsonSerializer.Serialize(data);
            }
            catch
            {
                return "";
            }
        }
    }
}
