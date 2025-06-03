using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Implementations
{
    public class AdminManagerServiceImpl : IAdminManagerService
    {
        public async Task<List<ManagerDTO>> GetListOfManagers()
        {
            return new List<ManagerDTO>() 
            {
                new ManagerDTO
                {
                    Name = "Test Name",
                    Email = "abcaf@gmail.com",
                    Role = "Manager"
                },
                new ManagerDTO
                {
                    Name = "Test Name",
                    Email = "abcaf@gmail.com",
                    Role = "Main Manager"
                },
            };
        }
    }
}
