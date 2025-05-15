using ApplicantPersonalAccount.Notification.Models;

namespace ApplicantPersonalAccount.Notification.Services
{
    public interface INotificationService
    {
        public Task SendEmail(string key, NotificationModel notification);
    }
}
