using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.DTOs.Filters;
using ApplicantPersonalAccount.DirectoryService.Services;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.DirectoryService.MessageListener
{
    public class FilterProgramListener : BaseMessageListener<FilterByProgramDTO>
    {
        public FilterProgramListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_FILTERED_PROGRAMS) { }

        protected override async Task<string?> ProcessMessage(
            FilterByProgramDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var filterService = serviceProvider.GetRequiredService<IFilterService>();

            var ids = await filterService.GetFilteredPrograms(message.Program!, message.Faculties!);

            return JsonSerializer.Serialize(ids);
        }
    }
}
