using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.UserAuth.Services.Implementations
{
    public class FilterServiceImpl : IFilterService
    {
        private readonly UserDataContext _userDataContext;

        public FilterServiceImpl(UserDataContext userDataContext)
        {
            _userDataContext = userDataContext;
        }

        public async Task<List<Guid>> GetFilteredIds(string name)
        {
            var ids = await _userDataContext.Users
                .Where(u => u.Name.Contains(name))
                .Select(u => u.Id)
                .ToListAsync();

            return ids;
        }
    }
}
