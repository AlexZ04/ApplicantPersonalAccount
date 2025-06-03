using ApplicantPersonalAccount.Common.DTOs.Managers;

namespace ApplicantPersonalAccount.UserAuth.Services
{
    public interface IManagerService
    {
        public Task<List<ManagerProfileDTO>> GetAllManagers();
        public Task DeleteManagerById(Guid id);
        public Task UpdateManager(ManagerUpdateDTO updateData);
    }
}
