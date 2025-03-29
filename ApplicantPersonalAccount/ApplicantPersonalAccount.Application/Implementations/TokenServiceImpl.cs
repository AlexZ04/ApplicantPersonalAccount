using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ApplicantPersonalAccount.Application.Implementations
{
    public class TokenServiceImpl : ITokenService
    {
        private JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private readonly UserDataContext _userDataContext;

        public TokenServiceImpl(UserDataContext userDataContext)
        {
            _userDataContext = userDataContext;
        }

        public string GenerateAccessToken(Guid id)
        {
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                });

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
            await _userDataContext.RefreshTokens
                .Include(t => t.User)
                .Where(t => t.User.Id == userId && t.Id == tokenId)
                .ExecuteDeleteAsync();
        }
    }
}
