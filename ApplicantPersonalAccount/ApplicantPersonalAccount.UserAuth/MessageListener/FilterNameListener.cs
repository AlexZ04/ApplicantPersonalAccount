using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.DTOs.Filters;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.UserAuth.Services;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class FilterNameListener : BaseMessageListener<BrokerRequestDTO>
    {
        public FilterNameListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_FILTERED_NAMES) { }

        protected override async Task<string?> ProcessMessage(
            BrokerRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var filterService = serviceProvider.GetRequiredService<IFilterService>();

            var ids = await filterService.GetFilteredIds(message.Request!);

            var data = new ListOfIdsDTO
            {
                Ids = ids
            };

            return JsonSerializer.Serialize(data);
        }
    }
}
