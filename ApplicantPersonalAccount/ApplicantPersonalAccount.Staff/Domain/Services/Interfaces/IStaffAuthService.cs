using ApplicantPersonalAccount.Staff.Models;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Interfaces
{
    public interface IStaffAuthService
    {
        public void Logout();
        public Task<bool> Login(LoginViewModel loginModel);
        public Task<bool> RefreshToken();
    }
}
