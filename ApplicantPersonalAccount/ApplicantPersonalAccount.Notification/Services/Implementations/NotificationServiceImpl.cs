using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Notification.Models;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.NotificationDb;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace ApplicantPersonalAccount.Notification.Services.Implementations
{
    public class NotificationServiceImpl : INotificationService
    {
        private readonly NotificationDataContext _notificationDataContext;
        private readonly ILogger<NotificationServiceImpl> _logger;

        public NotificationServiceImpl(
            NotificationDataContext notificationDataContext,
            ILogger<NotificationServiceImpl> logger)
        {
            _notificationDataContext = notificationDataContext;
            _logger = logger;
        }

        public async Task SendEmail(string key, NotificationModel notification)
        {
            if (key != NotificationsOptions.NOTIFICATION_KEY)
            {
                _logger.LogWarning($"Somebody typed invalid notification key");
                throw new UnaccessableAction(ErrorMessages.INVALID_KEY);
            }

            if (!(await CheckSubscription(notification.UserEmail)))
                return;

            try
            {
                await SendEmailBySmpt(notification);
            }
            catch (Exception)
            {
                _logger.LogWarning($"Can't send email to {notification.UserEmail}");
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
            _logger.LogInformation($"Sending email to {notification.UserEmail}");

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

        public async Task SignUserToNotifications(string userEmail)
        {
            var isSigned = await CheckSubscription(userEmail);

            if (isSigned)
            {
                _logger.LogError($"Can't sign {userEmail} to notifications");
                throw new InvalidActionException(ErrorMessages.USER_IS_SIGNED);
            }
            
            var subUser = new NotificationSubscribtionEntity
            {
                Id = Guid.NewGuid(),
                UserEmail = userEmail,
            };

            _notificationDataContext.Subscribers.Add(subUser);
            await _notificationDataContext.SaveChangesAsync();
        }

        public async Task UnsignUserFromNotifications(string userEmail)
        {
            var isSigned = await CheckSubscription(userEmail);

            if (!isSigned)
            {
                _logger.LogError($"Can't unsign {userEmail} from notifications");
                throw new InvalidActionException(ErrorMessages.USER_IS_UNSIGNED);
            }

            var record = await _notificationDataContext.Subscribers
                .FirstAsync(s => s.UserEmail == userEmail);

            _notificationDataContext.Subscribers.Remove(record);
            await _notificationDataContext.SaveChangesAsync();
        }
    }
}
