namespace ApplicantPersonalAccount.Application
{
    public interface ITokenService
    {
        public string GenerateAccessToken(Guid id);
        public string GenerateRefreshToken();
        public Task HandleTokens(Guid userId, Guid tokenId);
    }
}
