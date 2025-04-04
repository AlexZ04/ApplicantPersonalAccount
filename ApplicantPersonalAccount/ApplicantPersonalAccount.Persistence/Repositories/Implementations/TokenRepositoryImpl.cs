
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace ApplicantPersonalAccount.Persistence.Repositories.Implementations
{
    public class TokenRepositoryImpl : ITokenRepository
    {
        private readonly UserDataContext _userContext;
        private readonly IDistributedCache _tokenCache;

        public TokenRepositoryImpl(UserDataContext userContext, IDistributedCache tokenCache)
        {
            _userContext = userContext;
            _tokenCache = tokenCache;
        }

        public async Task HandleTokens(Guid userId, Guid tokenId)
        {
            await _userContext.RefreshTokens
                .Include(t => t.User)
                .Where(t => t.User.Id == userId && t.Id != tokenId)
                .ExecuteDeleteAsync();
        }

        public async Task CacheTokens(string accessToken, string refreshToken)
        {
            await _tokenCache.SetStringAsync($"blacklist:access:{accessToken}", "logout", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(GeneralSettings.ACCESS_TOKEN_LIFETIME)
            });

            await _tokenCache.SetStringAsync($"blacklist:refresh:{refreshToken}", "logout", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(GeneralSettings.REFRESH_TOKEN_LIFETIME)
            });
        }

        public async Task<RefreshTokenEntity> GetUserRefreshToken(Guid userId)
        {
            RefreshTokenEntity? refreshToken = await _userContext.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.User.Id == userId);

            if (refreshToken == null)
                throw new UnauthorizedAccessException();

            return refreshToken;
        }

        public async Task<bool> IsRefreshTokenValid(string refreshToken)
        {
            string? isTokenCached = await _tokenCache.GetStringAsync($"blacklist:refresh:{refreshToken}");

            if (isTokenCached == null)
                return true;
            return false;
        }
    }
}
