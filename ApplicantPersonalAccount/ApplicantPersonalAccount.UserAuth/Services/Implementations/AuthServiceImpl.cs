﻿using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using System.Security.Claims;

namespace ApplicantPersonalAccount.UserAuth.Services.Implementations
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IMessageProducer _messageProducer;

        public AuthServiceImpl(
            IUserRepository userRepository,
            ITokenService tokenService,
            ITokenRepository tokenRepository,
            IMessageProducer messageProducer)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
            _messageProducer = messageProducer;
        }

        public async Task<TokenResponseModel> RegisterUser(UserRegisterModel user)
        {
            if (!await _userRepository.EmailAndPhoneIsAvailable(user.Email, user.Phone))
                throw new ImpossibleActionException(ErrorMessages.CANT_REGISTER_USER);

            UserEntity newUser = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Gender = user.Gender,
                Birthdate = user.Birthdate,
                Address = user.Address,
                Citizenship = user.Citizenship,
                Password = Hasher.HashPassword(user.Password),
                CreateTime = DateTime.Now.ToUniversalTime(),
                UpdateTime = DateTime.Now.ToUniversalTime(),

                InfoForEvents = new InfoForEventsEntity
                {
                    Id = Guid.NewGuid(),
                    EducationPlace = string.Empty,
                    SocialNetwork = string.Empty
                }
            };

            _userRepository.AddUser(newUser);
            newUser.InfoForEvents.User = newUser;

            TokenResponseModel tokenResponseModel = GetTokenAndAddToDb(newUser);

            await _tokenService.HandleTokens(newUser.Id, Guid.Empty);

            await _userRepository.SaveChanges();

            _messageProducer.SendMessage(new GuidRequestDTO
            {
                Id = newUser.Id,
            }, RabbitQueues.CREATE_ENTERANCE);

            return tokenResponseModel;
        }

        public async Task<TokenResponseModel> LoginUser(UserLoginModel loginCredentials)
        {
            UserEntity user = await _userRepository.GetUsersByCredentials(
                loginCredentials.Email, loginCredentials.Password);

            TokenResponseModel refreshToken = GetTokenAndAddToDb(user);

            await _tokenService.HandleTokens(user.Id, Guid.Empty);

            await _userRepository.SaveChanges();

            return refreshToken;
        }

        public async Task<TokenResponseModel> LoginAdmin(UserLoginModel loginCredentials)
        {
            UserEntity user = await _userRepository.GetUsersByCredentials(
                loginCredentials.Email, loginCredentials.Password);

            if (user.Role == Role.Applicant)
                throw new UnaccessableAction(ErrorMessages.INVALID_CREDENTIALS);

            return await LoginUser(loginCredentials);
        }

        public async Task<TokenResponseModel> LoginRefresh(RefreshTokenModel tokenModel)
        {
            RefreshTokenEntity refreshToken = await _userRepository
                .GetRefreshToken(tokenModel.RefreshToken);

            if (!await _tokenRepository.IsRefreshTokenValid(refreshToken.Token))
                throw new UnauthorizedAccessException();

            string accessToken = _tokenService.GenerateAccessToken(refreshToken.User.Id,
                refreshToken.User.Role.ToString(), refreshToken.User.Email);

            refreshToken.Token = _tokenService.GenerateRefreshToken();
            refreshToken.Expires = DateTime.Now.AddDays(GeneralSettings.REFRESH_TOKEN_LIFETIME)
                .ToUniversalTime();

            await _tokenService.HandleTokens(refreshToken.User.Id, refreshToken.Id);

            await _userRepository.SaveChanges();

            return new TokenResponseModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessExpireTime = DateTime.Now.AddMinutes(AuthOptions.LIFETIME_MINUTES).ToUniversalTime(),
            };
        }

        public async Task Logout(string? token, Guid userId)
        {
            if (token == null)
                throw new UnauthorizedAccessException();

            RefreshTokenEntity refreshToken = await _tokenRepository
                .GetUserRefreshToken(userId);

            await _tokenService.CacheTokens(token, refreshToken.Token);
        }

        private TokenResponseModel GetTokenAndAddToDb(UserEntity user)
        {
            RefreshTokenEntity refreshToken = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                User = user,
                Token = _tokenService.GenerateRefreshToken(),
                Expires = DateTime.Now.AddHours(GeneralSettings.REFRESH_TOKEN_LIFETIME)
                    .ToUniversalTime()
            };

            _userRepository.AddRefreshToken(refreshToken);

            TokenResponseModel tokenResponseModel = new TokenResponseModel
            {
                AccessToken = _tokenService.GenerateAccessToken(user.Id, user.Role.ToString(), user.Email),
                RefreshToken = refreshToken.Token,
                AccessExpireTime = DateTime.Now.AddMinutes(GeneralSettings.ACCESS_TOKEN_LIFETIME)
                    .ToUniversalTime()
            };

            return tokenResponseModel;
        }
    }
}
