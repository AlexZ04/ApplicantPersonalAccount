using ApplicantPersonalAccount.Notification.Models;

namespace ApplicantPersonalAccount.Notification.Services
{
    public interface INotificationService
    {
        public Task SendEmail(string key, NotificationModel notification);
        public Task SignUserToNotifications(string userEmail);
        public Task UnsignUserFromNotifications(string userEmail);
    }
}
