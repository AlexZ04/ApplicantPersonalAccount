using ApplicantPersonalAccount.Persistence.Entities.UsersDb;

namespace ApplicantPersonalAccount.Persistence.Repositories
{
    public interface ITokenRepository
    {
        public Task HandleTokens(Guid userId, Guid tokenId);
        public Task CacheTokens(string accessToken, string refreshToken);
        public Task<RefreshTokenEntity> GetUserRefreshToken(Guid userId);
    }
}
