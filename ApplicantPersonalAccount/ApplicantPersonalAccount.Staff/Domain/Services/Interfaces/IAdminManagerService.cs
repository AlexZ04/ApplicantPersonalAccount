using ApplicantPersonalAccount.Common.DTOs.Managers;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Interfaces
{
    public interface IAdminManagerService
    {
        public Task<List<ManagerDTO>> GetListOfManagers();
    }
}
