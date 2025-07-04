﻿using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ApplicantPersonalAccount.UserAuth.Services.Implementations
{
    public class TokenServiceImpl : ITokenService
    {
        private JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private readonly ITokenRepository _tokenRepository;

        public TokenServiceImpl(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public string GenerateAccessToken(Guid id, string role, string email)
        {
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Issuer = AuthOptions.ISSUER,
                Audience = AuthOptions.AUDIENCE,
                Expires = DateTime.Now.AddMinutes(AuthOptions.LIFETIME_MINUTES).ToUniversalTime(),
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
