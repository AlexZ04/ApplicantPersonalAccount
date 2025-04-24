using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;

namespace ApplicantPersonalAccount.Persistence.Repositories
{
    public interface IApplicationRepository
    {
        public Task<bool> IsUserSigned(Guid userId);
        public Task SignUser(SignedToNotificationsEntity signingInfo);
        public Task UnsignUser(Guid userId);
    }
}
