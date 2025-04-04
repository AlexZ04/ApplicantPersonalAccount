using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ApplicantPersonalAccount.Application.Implementations
{
    public class TokenServiceImpl : ITokenService
    {
        private JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private readonly ITokenRepository _tokenRepository;

        public TokenServiceImpl(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public string GenerateAccessToken(Guid id, string role)
        {
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                });

            claims.AddClaim(new Claim(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Issuer = AuthOptions.ISSUER,
                Audience = AuthOptions.AUDIENCE,
                Expires = DateTime.UtcNow.AddMinutes(AuthOptions.LIFETIME_MINUTES),
                SigningCredentials = new(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public async Task HandleTokens(Guid userId, Guid tokenId)
        {
            await _tokenRepository.HandleTokens(userId, tokenId);
        }

        public async Task CacheTokens(string accessToken, string refreshToken)
        {
            await _tokenRepository.CacheTokens(accessToken, refreshToken);
        }
    }
}
