using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

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
            _userContext.InfoForEvents.Add(user.InfoForEvents);
        }

        public async Task<bool> EmailIsAvailable(string email)
        {
            UserEntity? user = await _userContext.Users
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
            UserEntity? user = await _userContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !Hasher.CheckPassword(user.Password, password))
                throw new InvalidActionException(ErrorMessages.INVALID_CREDENTIALS);

            return user;
        }

        public async Task<RefreshTokenEntity> GetRefreshToken(string token)
        {
            RefreshTokenEntity? refreshToken = await _userContext.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == token);

            if (refreshToken == null || refreshToken.Expires < DateTime.Now.ToUniversalTime())
                throw new UnauthorizedAccessException();

            return refreshToken;
        }

        public async Task<UserEntity> GetUserById(Guid id)
        {
            UserEntity? user = await _userContext.Users
                .FindAsync(id);

            return user != null ? user : throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);
        }

        public async Task EditInfoForEvents(EditApplicantInfoForEventsModel editedInfo, Guid userId)
        {
            var info = await _userContext.InfoForEvents
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.User.Id == userId);

            if (info == null)
                throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);

            info.EducationPlace = editedInfo.EducationPlace;
            info.SocialNetwork = editedInfo.SocialNetworks;

            await _userContext.SaveChangesAsync();
        }

        public async Task<InfoForEventsEntity> GetInfoForEvents(Guid userId)
        {
            var info = await _userContext.InfoForEvents
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.User.Id == userId);

            return info ?? throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);
        }
    }
}
