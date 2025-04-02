using ApplicantPersonalAccount.Common.Models.User;
using System.Security.Claims;

namespace ApplicantPersonalAccount.Application
{
    public interface IUserService
    {
        public Task<UserProfileModel> GetProfile(ClaimsPrincipal user);
    }
}
