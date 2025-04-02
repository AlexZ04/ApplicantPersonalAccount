using ApplicantPersonalAccount.Common.Models.Authorization;

namespace ApplicantPersonalAccount.Application
{
    public interface IAuthService
    {
        public Task<TokenResponseModel> RegisterUser(UserRegisterModel user);
        public Task<TokenResponseModel> LoginUser(UserLoginModel loginCredentials);
        public Task<TokenResponseModel> LoginRefresh(RefreshTokenModel tokenModel);
    }
}
