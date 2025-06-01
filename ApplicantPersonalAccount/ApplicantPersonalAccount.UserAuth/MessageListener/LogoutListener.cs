using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.UserAuth.Services;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class LogoutListener : BaseMessageListener<LogoutDTO>
    {
        public LogoutListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.LOGOUT) { }

        protected override async Task<string?> ProcessMessage(
            LogoutDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var authService = serviceProvider.GetRequiredService<IAuthService>();

            try
            {
                await authService.Logout(message.Token, message.UserId);

                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
