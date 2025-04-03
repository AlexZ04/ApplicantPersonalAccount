using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.User;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using System.Security.Claims;

namespace ApplicantPersonalAccount.Application.Implementations
{
    public class UserServiseImpl : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserServiseImpl(IUserRepository userRepository)
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

        public async Task ChangePassword(PasswordEditModel passwordModel, ClaimsPrincipal user)
        {
            UserEntity foundUser = await _userRepository.GetUserById(UserDescriptor.GetUserId(user));

            if (!Hasher.CheckPassword(foundUser.Password, passwordModel.OldPassword))
                throw new ImpossibleActionException(ErrorMessages.INVALID_PASSWORD);

            foundUser.Password = Hasher.HashPassword(passwordModel.Password);

            await _userRepository.SaveChanges();
        }

        public async Task ChangeEmail(EmailEditModel passwordModel, ClaimsPrincipal user)
        {
            UserEntity foundUser = await _userRepository.GetUserById(UserDescriptor.GetUserId(user));

            foundUser.Email = passwordModel.Email;
            foundUser.UpdateTime = DateTime.UtcNow.ToUniversalTime();

            await _userRepository.SaveChanges();
        }

        public async Task EditProfile(UserEditModel userNewInfo, ClaimsPrincipal user)
        {
            UserEntity foundUser = await _userRepository.GetUserById(UserDescriptor.GetUserId(user));

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
    }
}
