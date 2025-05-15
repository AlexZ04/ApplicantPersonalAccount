using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Notification.Models;
using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace ApplicantPersonalAccount.Notification.Services.Implementations
{
    public class NotificationServiceImpl : INotificationService
    {
        private readonly NotificationDataContext _notificationDataContext;

        public NotificationServiceImpl(NotificationDataContext notificationDataContext)
        {
            _notificationDataContext = notificationDataContext;
        }

        public async Task SendEmail(string key, NotificationModel notification)
        {
            if (key != NotificationsOptions.NOTIFICATION_KEY)
                throw new UnaccessableAction(ErrorMessages.INVALID_KEY);

            if (!(await CheckSubscription(notification.UserEmail)))
                return;

            try
            {
                await SendEmail(notification);
            }
            catch (Exception)
            {
                throw new InvalidActionException(ErrorMessages.CANT_SEND_EMAIL);
            }
        }

        private async Task<bool> CheckSubscription(string email)
        {
            var foundUser = await _notificationDataContext.Subscribers
                .FirstOrDefaultAsync(s => s.UserEmail == email);

            return foundUser != null;
        }

        private async Task SendEmailBySmpt(NotificationModel notification)
        {
            MailAddress to = new MailAddress(notification.UserEmail);
            MailAddress from = new MailAddress(NotificationsOptions.EMAIL, 
                NotificationsOptions.FROM_TITLE);

            MailMessage message = new MailMessage(from, to);

            message.Subject = notification.Title;
            message.Body = notification.Text;

            message.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Timeout = 10000;
            smtp.Credentials = new NetworkCredential(NotificationsOptions.EMAIL, 
                NotificationsOptions.APP_PASSWORD);

            await smtp.SendMailAsync(message);
        }
    }
}
