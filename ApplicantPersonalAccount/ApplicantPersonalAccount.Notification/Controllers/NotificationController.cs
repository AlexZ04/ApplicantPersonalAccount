using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Notification.Models;
using ApplicantPersonalAccount.Notification.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Notification.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMessageProducer _messageProducer;

        public NotificationController(INotificationService notificationService,
            IMessageProducer messageProducer)
        {
            _notificationService = notificationService;
            _messageProducer = messageProducer;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToNotifications(string userEmail)
        {
            await _notificationService.SignUserToNotifications(userEmail);

            return Ok();
        }

        [HttpDelete("subscribe")]
        public async Task<IActionResult> UnsubscribeFromNotifications(string userEmail)
        {
            await _notificationService.UnsignUserFromNotifications(userEmail);

            return Ok();
        }

        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification(
            [FromQuery, Required] string key,
            [FromBody] NotificationModel notification)
        {
            _messageProducer.SendMessage(notification, "notification_queue");
            //await _notificationService.SendEmail(key, notification);

            return Ok();
        }
    }
}
