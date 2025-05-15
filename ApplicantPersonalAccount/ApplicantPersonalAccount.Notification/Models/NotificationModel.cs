using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Notification.Models
{
    public class NotificationModel
    {
        [Required, EmailAddress]
        public string UserEmail { get; set; }
        [Required, MinLength(1)]
        public string Title { get; set; }
        [Required, MinLength(1)]
        public string Text { get; set; }
    }
}
