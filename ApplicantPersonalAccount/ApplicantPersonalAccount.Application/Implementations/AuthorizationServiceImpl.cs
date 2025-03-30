using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using ApplicantPersonalAccount.Persistence.Repositories;

namespace ApplicantPersonalAccount.Application.Implementations
{
    public class AuthorizationServiceImpl : IAuthorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthorizationServiceImpl(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<TokenResponseModel> RegisterUser(UserRegisterModel user)
        {
            if (!await _userRepository.EmailIsAvailable(user.Email))
                throw new ImpossibleActionException(ErrorMessages.CANT_REGISTER_USER);

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
                Password = Hasher.HashPassword(user.Password),
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

            TokenResponseModel tokenResponseModel = GetTokenAndAddToDb(newUser);

            await _userRepository.SaveChanges();

            return tokenResponseModel;
        }

        public async Task<TokenResponseModel> LoginUser(UserLoginModel loginCredentials)
        {
            var user = await _userRepository.GetUsersByCredentials(
                loginCredentials.Email, loginCredentials.Password);

            TokenResponseModel refreshToken = GetTokenAndAddToDb(user);

            await _userRepository.SaveChanges();

            return refreshToken;
        }

        private TokenResponseModel GetTokenAndAddToDb(UserEntity user)
        {
            RefreshTokenEntity refreshToken = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                User = user,
                Token = _tokenService.GenerateRefreshToken(),
                Expires = DateTime.Now.AddHours(GeneralSettings.REFRESH_TOKEN_LIFETIME).ToUniversalTime()
            };

            _userRepository.AddRefreshToken(refreshToken);

            TokenResponseModel tokenResponseModel = new TokenResponseModel
            {
                AccessToken = _tokenService.GenerateAccessToken(user.Id),
                RefreshToken = refreshToken.Token,
                AccessExpireTime = DateTime.Now.AddMinutes(GeneralSettings.ACCESS_TOKEN_LIFETIME).ToUniversalTime()
            };

            return tokenResponseModel;
        }
    }
}
