using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.UserAuth.Services.Implementations
{
    public class IManagerServiceImpl : IManagerService
    {
        private readonly UserDataContext _userContext;

        public IManagerServiceImpl(UserDataContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<List<ManagerProfileDTO>> GetAllManagers()
        {
            var managers = await _userContext.Users
                .Where(u => u.Role == Role.Manager || u.Role == Role.HeadManager)
                .Select(m => new ManagerProfileDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    Phone = m.Phone,
                    Gender = m.Gender,
                    Birthdate = m.Birthdate,
                    Address = m.Address,
                    Citizenship = m.Citizenship,
                    Role = m.Role,
                    CreateTime = m.CreateTime,
                    UpdateTime = m.UpdateTime,
                })
                .ToListAsync();

            return managers;
        }
    }
}
