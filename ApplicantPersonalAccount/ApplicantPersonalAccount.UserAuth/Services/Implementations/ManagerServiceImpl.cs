using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using ApplicantPersonalAccount.Notification.Models;

namespace ApplicantPersonalAccount.UserAuth.Services.Implementations
{
    public class ManagerServiceImpl : IManagerService
    {
        private readonly UserDataContext _userContext;
        private readonly IUserRepository _userRepository;
        private readonly IMessageProducer _messageProducer;
        private readonly ILogger<ManagerServiceImpl> _logger;

        public ManagerServiceImpl(
            UserDataContext userContext,
            IUserRepository userRepository,
            IMessageProducer messageProducer,
            ILogger<ManagerServiceImpl> logger)
        {
            _userContext = userContext;
            _userRepository = userRepository;
            _messageProducer = messageProducer;
            _logger = logger;
        }

        public async Task<List<ManagerProfileDTO>> GetAllManagers()
        {
            _logger.LogInformation("Getting list of all managers");

            var managers = await _userContext.Users
                .Where(u => u.Role == Role.Manager || u.Role == Role.HeadManager)
                .Select(m => new ManagerProfileDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    Phone = m.Phone,
                    Gender = m.Gender,
                    Birthdate = m.Birthdate,
                    Address = m.Address,
                    Citizenship = m.Citizenship,
                    Role = m.Role,
                    CreateTime = m.CreateTime,
                    UpdateTime = m.UpdateTime,
                })
                .AsNoTracking()
                .ToListAsync();

            return managers;
        }

        public async Task DeleteManagerById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);

            _logger.LogInformation($"Deleting manager with id {id}");

            _userContext.Users.Remove(user);
            await _userContext.SaveChangesAsync();
        }

        public async Task UpdateManager(ManagerUpdateDTO updateData)
        {
            var manager = await _userRepository.GetUserById(updateData.Id);

            _logger.LogInformation($"Updating manager with id {updateData.Id}");

            manager.Name = updateData.Name;
            manager.Email = updateData.Email;
            manager.Phone = updateData.Phone;
            manager.Gender = updateData.Gender;

            await _userContext.SaveChangesAsync();
        }

        public async Task<bool> CreateManager(ManagerCreateDTO createManager)
        {
            var canBeCreated = await _userRepository.EmailIsAvailable(createManager.Email);

            if (!canBeCreated)
                return false;

            var newUser = new UserEntity
            {
                Id = createManager.Id,
                Name = createManager.Name,
                Email = createManager.Email,
                Phone = createManager.Phone,
                Gender = createManager.Gender,
                Birthdate = createManager.Birthday.ToUniversalTime(),
                Password = Hasher.HashPassword(createManager.Password),
                CreateTime = DateTime.Now.ToUniversalTime(),
                UpdateTime = DateTime.Now.ToUniversalTime(),
                Role = createManager.Role,

                InfoForEvents = new InfoForEventsEntity
                {
                    Id = Guid.NewGuid(),
                    EducationPlace = string.Empty,
                    SocialNetwork = string.Empty
                }
            };

            _userRepository.AddUser(newUser);
            newUser.InfoForEvents.User = newUser;

            await _userRepository.SaveChanges();

            //SendGreetingsEmail(newUser, createManager.Password);
            return true;
        }

        private void SendGreetingsEmail(UserEntity user, string password)
        {
            var notification = new NotificationModel
            {
                UserEmail = user.Email,
                Title = "Welcome to team",
                Text = FormGreetingsManagerText(user, password)
            };

            _messageProducer.SendMessage(notification, RabbitQueues.NOTIFICATION);
        }

        private string FormGreetingsManagerText(UserEntity user, string password)
        {
            var message = $"Hey, {user.Name}! Welcome to team. \n" +
                $"Now you are the {user.Role} in ApplicantPersonalAccount company.\n\n" +
                $"You credentials is: \n" +
                $"Email: {user.Email}\n" +
                $"Password: {password}\n\n" +
                $"Don't show this email to anyone. Edit your password in security considerations";

            return message;
        }
    }
}
