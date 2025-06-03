using ApplicantPersonalAccount.Common.DTOs.Managers;

namespace ApplicantPersonalAccount.UserAuth.Services
{
    public interface IManagerService
    {
        public Task<List<ManagerProfileDTO>> GetAllManagers();

    }
}
