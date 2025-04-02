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
    }
}
