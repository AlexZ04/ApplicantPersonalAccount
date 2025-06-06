using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;
using ApplicantPersonalAccount.Staff.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using ApplicantPersonalAccount.Common.Constants;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Implementations
{
    public class StaffAuthServiceImpl : IStaffAuthService
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StaffAuthServiceImpl(
            IMessageProducer messageProducer,
            IHttpContextAccessor httpContextAccessor)
        {
            _messageProducer = messageProducer;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Logout(Guid userId)
        {
            var context = _httpContextAccessor.HttpContext;

            var logoutDto = new LogoutDTO
            {
                UserId = userId,
                Token = context?.Request.Cookies["RefreshToken"]
            };

            _messageProducer.SendMessage(logoutDto, RabbitQueues.LOGOUT);

            if (context != null)
            {
                context.Response.Cookies.Delete("AccessToken");
                context.Response.Cookies.Delete("RefreshToken");
            }
        }

        public void DeleteCookies()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null)
            {
                context.Response.Cookies.Delete("AccessToken");
                context.Response.Cookies.Delete("RefreshToken");
            }
        }

        public async Task<bool> Login(LoginViewModel loginModel)
        {
            var rpcClient = new RpcClient();
            var request = new UserLoginModel
            {
                Email = loginModel.UserEmail!,
                Password = loginModel.Password!
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.LOGIN);
            rpcClient.Dispose();

            if (result == "")
                return false;

            var tokenData = JsonSerializer.Deserialize<TokenResponseModel>(result)!;
            SetAuthCookies(tokenData);

            return true;
        }

        public async Task<TokenResponseModel?> RefreshToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return null;

            var refreshToken = context.Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) return null;

            var rpcClient = new RpcClient();
            var request = new RefreshTokenModel
            {
                RefreshToken = refreshToken
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.REFRESH_LOGIN);
            rpcClient.Dispose();

            if (result == "")
                return null;

            var tokenData = JsonSerializer.Deserialize<TokenResponseModel>(result)!;
            SetAuthCookies(tokenData);

            return tokenData;
        }

        private void SetAuthCookies(TokenResponseModel tokenData)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return;

            AddTokenToCookie(context, tokenData);
            AddTokenToCookie(context, tokenData, false);
        }

        private void AddTokenToCookie(
            HttpContext context,
            TokenResponseModel tokenData,
            bool isAccess = true)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            if (isAccess)
            {
                cookieOptions.Expires = tokenData.AccessExpireTime;
                context.Response.Cookies.Append("AccessToken", tokenData.AccessToken, cookieOptions);
                return;
            }

            cookieOptions.Expires = DateTime.Now.AddDays(GeneralSettings.REFRESH_TOKEN_LIFETIME);
            context.Response.Cookies.Append("RefreshToken", tokenData.RefreshToken, cookieOptions);
        }
    }
}
