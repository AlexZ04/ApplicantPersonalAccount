using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Common.Models.User;

namespace ApplicantPersonalAccount.UserAuth.Services
{
    public interface IManagerService
    {
        public Task<List<ManagerProfileDTO>> GetAllManagers();
        public Task DeleteManagerById(Guid id);
        public Task UpdateManager(ManagerUpdateDTO updateData);
        public Task<bool> CreateManager(ManagerCreateDTO createManager);
        public Task EditUser(ApplicantEditModel newInfo, Guid userId, string currentUserRole);
    }
}
