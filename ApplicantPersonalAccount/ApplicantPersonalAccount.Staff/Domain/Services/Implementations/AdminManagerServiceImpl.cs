using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Implementations
{
    public class AdminManagerServiceImpl : IAdminManagerService
    {
        public async Task<List<ManagerDTO>> GetListOfManagers()
        {
            return new List<ManagerDTO>();
        }
    }
}
