using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.UserAuth.Services;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class LoginListener : BaseMessageListener<UserLoginModel>
    {
        public LoginListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.LOGIN) { }

        protected override async Task<string?> ProcessMessage(
            UserLoginModel message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var authService = serviceProvider.GetRequiredService<IAuthService>();

            try
            {
                var response = await authService.LoginUser(message);
                return JsonSerializer.Serialize(response);
            }
            catch
            {
                return "";
            }
        }
    }
}
