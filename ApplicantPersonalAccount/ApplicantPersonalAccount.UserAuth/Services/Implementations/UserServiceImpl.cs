using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.User;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using System.Security.Claims;

namespace ApplicantPersonalAccount.UserAuth.Services.Implementations
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserServiceImpl(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileModel> GetProfile(ClaimsPrincipal user)
        {
            UserEntity foundUser = await _userRepository.GetUserById(UserDescriptor.GetUserId(user));

            UserProfileModel profile = new UserProfileModel
            {
                Id = foundUser.Id,
                Name = foundUser.Name,
                Role = foundUser.Role,
                Email = foundUser.Email,
                Phone = foundUser.Phone,
                Gender = foundUser.Gender,
                Birthdate = foundUser.Birthdate,
                Citizenship = foundUser.Citizenship,
                Address = foundUser.Address,
            };

            return profile;
        }

        public async Task ChangePassword(PasswordEditModel passwordModel, Guid userId)
        {
            UserEntity foundUser = await _userRepository.GetUserById(userId);

            if (!Hasher.CheckPassword(foundUser.Password, passwordModel.OldPassword))
                throw new ImpossibleActionException(ErrorMessages.INVALID_PASSWORD);

            foundUser.Password = Hasher.HashPassword(passwordModel.Password);

            await _userRepository.SaveChanges();
        }

        public async Task ChangeEmail(EmailEditModel passwordModel, Guid userId)
        {
            UserEntity foundUser = await _userRepository.GetUserById(userId);

            foundUser.Email = passwordModel.Email;
            foundUser.UpdateTime = DateTime.UtcNow.ToUniversalTime();

            await _userRepository.SaveChanges();
        }

        public async Task EditProfile(UserEditModel userNewInfo, Guid userId, string userRole)
        {
            UserEntity foundUser = await _userRepository.GetUserById(userId);

            if (userRole == "Applicant")
                await CheckEditable(userId);

            foundUser.Name = userNewInfo.Name;
            foundUser.Email = userNewInfo.Email;
            foundUser.Phone = userNewInfo.Phone;
            foundUser.Gender = userNewInfo.Gender;
            foundUser.Birthdate = userNewInfo.Birthdate;
            foundUser.Citizenship = userNewInfo.Citizenship;
            foundUser.Address = userNewInfo.Address;

            foundUser.UpdateTime = DateTime.UtcNow.ToUniversalTime();

            await _userRepository.SaveChanges();
        }

        private async Task CheckEditable(Guid userId)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = userId
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.CAN_EDIT_LISTENER);
            if (result == null || result == "false")
                throw new InvalidActionException(ErrorMessages.USER_CANT_EDIT_THIS_DATA);
        }
    }
}
