using ApplicantPersonalAccount.Staff.Models;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Interfaces
{
    public interface IStaffAuthService
    {
        public Task Logout();
        public Task<bool> Login(LoginViewModel loginModel);
    }
}
