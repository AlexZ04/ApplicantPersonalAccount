using ApplicantPersonalAccount.Common.Models.Authorization;

namespace ApplicantPersonalAccount.Application
{
    public interface IAuthorizationService
    {
        public Task<TokenResponseModel> RegisterUser(UserRegisterModel user);
        public Task<TokenResponseModel> LoginUser(UserLoginModel loginCredentials);
    }
}
