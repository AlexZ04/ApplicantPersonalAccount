namespace ApplicantPersonalAccount.UserAuth.Services
{
    public interface ITokenService
    {
        public string GenerateAccessToken(Guid id, string role, string email);
        public string GenerateRefreshToken();
        public Task HandleTokens(Guid userId, Guid tokenId);
        public Task CacheTokens(string accessToken, string refreshToken);
    }
}
