using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Staff.Models;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Interfaces
{
    public interface IAdminManagerService
    {
        public Task<List<ManagerDTO>> GetListOfManagers();
        public Task<ManagerProfileViewModel> GetManagerProfile(Guid id);
        public void DeleteManager(Guid id);
        public void EditManagerProfile(ManagerProfileViewModel model);
        public Task<bool> CreateManager(ManagerCreateModel model);
    }
}
