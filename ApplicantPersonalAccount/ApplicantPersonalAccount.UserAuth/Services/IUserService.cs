using ApplicantPersonalAccount.Common.Models.User;
using System.Security.Claims;

namespace ApplicantPersonalAccount.UserAuth.Services
{
    public interface IUserService
    {
        public Task<UserProfileModel> GetProfile(Guid userId);
        public Task ChangePassword(PasswordEditModel passwordModel, Guid userId);
        public Task ChangeEmail(EmailEditModel passwordModel, Guid userId);
        public Task EditProfile(UserEditModel userNewInfo, Guid userId, string userRole);
    }
}
