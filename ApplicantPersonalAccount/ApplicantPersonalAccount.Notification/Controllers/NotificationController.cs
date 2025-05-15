using ApplicantPersonalAccount.Notification.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.Notification.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : ControllerBase
    {
        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification(
            [FromQuery] string key,
            [FromBody] NotificationModel notification)
        {
            return Ok();
        }
    }
}
