using ApplicantPersonalAccount.Common.Models;

namespace ApplicantPersonalAccount.Application
{
    public interface IAuthorizationService
    {
        public Task<TokenResponseModel> RegisterUser(UserRegisterModel user);
    }
}
