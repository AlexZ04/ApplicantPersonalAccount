﻿using ApplicantPersonalAccount.Common.Models.Authorization;
using System.Security.Claims;

namespace ApplicantPersonalAccount.UserAuth.Services
{
    public interface IAuthService
    {
        public Task<TokenResponseModel> RegisterUser(UserRegisterModel user);
        public Task<TokenResponseModel> LoginUser(UserLoginModel loginCredentials);
        public Task<TokenResponseModel> LoginAdmin(UserLoginModel loginCredentials);
        public Task<TokenResponseModel> LoginRefresh(RefreshTokenModel tokenModel);
        public Task Logout(string? token, Guid userId);
    }
}
