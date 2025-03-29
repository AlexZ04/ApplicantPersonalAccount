using ApplicantPersonalAccount.Common.Models;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using ApplicantPersonalAccount.Persistence.Repositories;

namespace ApplicantPersonalAccount.Application.Implementations
{
    public class AuthorizationServiceImpl : IAuthorizationService
    {
        private readonly IUserRepository _userRepository;

        public AuthorizationServiceImpl(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<TokenResponseModel> RegisterUser(UserRegisterModel user)
        {
            if (!await _userRepository.EmailIsAvailable(user.Email))
            {
                // todo
            }

            UserEntity newUser = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Gender = user.Gender,
                Birthdate = user.Birthdate,
                Address = user.Address,
                Citizenship = user.Citizenship,
                Password = "",
                CreateTime = DateTime.Now.ToUniversalTime(),
                UpdateTime = DateTime.Now.ToUniversalTime(),

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

            return new TokenResponseModel
            {
                AccessToken = "",
                RefreshToken = "",
                AccessExpireTime = DateTime.Now,
            };
        }
    }
}
