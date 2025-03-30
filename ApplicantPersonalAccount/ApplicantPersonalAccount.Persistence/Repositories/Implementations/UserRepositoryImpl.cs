using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Persistence.Repositories.Implementations
{
    public class UserRepositoryImpl : IUserRepository 
    {
        private readonly UserDataContext _userContext;

        public UserRepositoryImpl(UserDataContext userContext)
        {
            _userContext = userContext;
        }

        public void AddUser(UserEntity user)
        {
            _userContext.Users.Add(user);
        }

        public async Task<bool> EmailIsAvailable(string email)
        {
            var user = await _userContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            return user == null;
        }

        public void AddRefreshToken(RefreshTokenEntity refreshToken)
        {
            _userContext.RefreshTokens.Add(refreshToken);
        }

        public async Task SaveChanges()
        {
            await _userContext.SaveChangesAsync();
        }

        public async Task<UserEntity> GetUsersByCredentials(string email, string password)
        {
            var user = await _userContext.Users
                .FirstOrDefaultAsync(u => u.Email == email &&
                Hasher.CheckPassword(u.Password, password));

            if (user == null)
                throw new InvalidActionException(ErrorMessages.INVALID_CREDENTIALS);

            return user;
        }
    }
}
