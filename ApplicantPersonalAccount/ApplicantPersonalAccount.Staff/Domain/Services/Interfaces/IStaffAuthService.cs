using ApplicantPersonalAccount.Staff.Models;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Interfaces
{
    public interface IStaffAuthService
    {
        public void Logout(Guid userId);
        public Task<bool> Login(LoginViewModel loginModel);
        public void DeleteCookies();
        public Task<bool> RefreshToken();
    }
}
