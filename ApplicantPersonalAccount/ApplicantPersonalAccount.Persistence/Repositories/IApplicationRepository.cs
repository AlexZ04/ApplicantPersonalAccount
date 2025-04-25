using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;

namespace ApplicantPersonalAccount.Persistence.Repositories
{
    public interface IApplicationRepository
    {
        public Task<bool> IsUserSigned(Guid userId);
        public Task SignUser(SignedToNotificationsEntity signingInfo);
        public Task UnsignUser(Guid userId);
        public Task DeleteProgram(Guid programId, Guid userId);
        public Task<EnteranceEntity> GetUserEnterance(Guid userId);
        public Task AddProgramToEnterance(EnteranceProgramEntity program, EnteranceEntity enterance);
    }
}
