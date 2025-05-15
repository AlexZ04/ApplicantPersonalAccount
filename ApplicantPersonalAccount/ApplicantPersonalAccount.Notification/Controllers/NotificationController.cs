using ApplicantPersonalAccount.Notification.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Notification.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : ControllerBase
    {
        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification(
            [FromQuery, Required] string key,
            [FromBody] NotificationModel notification)
        {
            return Ok();
        }
    }
}
