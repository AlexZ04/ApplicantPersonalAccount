using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.UserAuth.Services.Implementations
{
    public class ManagerServiceImpl : IManagerService
    {
        private readonly UserDataContext _userContext;

        public ManagerServiceImpl(UserDataContext userContext)
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
                .AsNoTracking()
                .ToListAsync();

            return managers;
        }

        public async Task DeleteManagerById(Guid id)
        {
            var user = await _userContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);

            _userContext.Users.Remove(user);
            await _userContext.SaveChangesAsync();
        }
    }
}
