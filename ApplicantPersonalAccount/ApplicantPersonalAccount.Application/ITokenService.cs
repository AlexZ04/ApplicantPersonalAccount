namespace ApplicantPersonalAccount.Application
{
    public interface ITokenService
    {
        public string GenerateAccessToken(Guid id, string role);
        public string GenerateRefreshToken();
        public Task HandleTokens(Guid userId, Guid tokenId);
    }
}
