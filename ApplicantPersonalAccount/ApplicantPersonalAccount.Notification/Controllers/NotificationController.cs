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

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
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
            await _notificationService.SendEmail(key, notification);

            return Ok();
        }
    }
}
