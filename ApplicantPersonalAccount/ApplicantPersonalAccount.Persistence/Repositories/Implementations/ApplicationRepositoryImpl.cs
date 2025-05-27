using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

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

        public async Task DeleteProgram(Guid programId, Guid userId)
        {
            var record = await _applicationContext.EnterancePrograms
                .Include(p => p.Enterance)
                .FirstOrDefaultAsync(p => p.ProgramId == programId && p.Enterance.ApplicantId == userId);

            if (record == null)
                throw new NotFoundException(ErrorMessages.PROGRAM_IS_NOT_FOUND);

            record.Enterance.Programs.Remove(record);
            _applicationContext.Enterances.Remove(record.Enterance);

            await _applicationContext.SaveChangesAsync();
        }

        public async Task<EnteranceEntity> GetUserEnterance(Guid userId, bool createIfNecessary = false)
        {
            var enterance = await _applicationContext.Enterances
                .Include(p => p.Programs)
                .FirstOrDefaultAsync(e => e.ApplicantId == userId);

            if (enterance == null && createIfNecessary)
                enterance = await CreateEnterance(userId);


            return enterance ?? throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);
        }

        public async Task AddProgramToEnterance(EnteranceProgramEntity program, EnteranceEntity enterance)
        {
            enterance.Programs.Add(program);
            _applicationContext.EnterancePrograms.Add(program);

            await _applicationContext.SaveChangesAsync();
        }

        private async Task<EnteranceEntity> CreateEnterance(Guid userId)
        {
            var newEnterance = new EnteranceEntity
            {
                ApplicantId = userId,
            };

            _applicationContext.Enterances.Add(newEnterance);
            await _applicationContext.SaveChangesAsync();

            return newEnterance;
        }
    }
}
