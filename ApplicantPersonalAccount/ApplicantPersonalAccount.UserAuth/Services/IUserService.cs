using ApplicantPersonalAccount.Common.Models.User;
using System.Security.Claims;

namespace ApplicantPersonalAccount.UserAuth.Services
{
    public interface IUserService
    {
        public Task<UserProfileModel> GetProfile(ClaimsPrincipal user);
        public Task ChangePassword(PasswordEditModel passwordModel, ClaimsPrincipal user);
        public Task ChangeEmail(EmailEditModel passwordModel, ClaimsPrincipal user);
        public Task EditProfile(UserEditModel userNewInfo, ClaimsPrincipal user);
    }
}
