using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Persistence.Repositories.Implementations
{
    public class ApplicationRepositoryImpl : IApplicationRepository
    {
        private readonly ApplicationDataContext _applicationContext;

        public ApplicationRepositoryImpl(ApplicationDataContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<bool> IsUserSigned(Guid userId)
        {
            var record = await _applicationContext.SignedToNotifications
                .FirstOrDefaultAsync(s => s.UserId == userId);

            return record != null;
        }
        public async Task SignUser(SignedToNotificationsEntity signingInfo)
        {
            _applicationContext.SignedToNotifications.Add(signingInfo);
            
            await _applicationContext.SaveChangesAsync();
        }

        public async Task UnsignUser(Guid userId)
        {
            var record = _applicationContext.SignedToNotifications
                .First(s => s.UserId == userId);

            _applicationContext.SignedToNotifications.Remove(record);

            await _applicationContext.SaveChangesAsync();
        }
    }
}
