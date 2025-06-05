using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.Enterance;

namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IEnteranceService
    {
        public Task<EnteranceModel> GetEnteranceByUserId(Guid userId);
        public Task<ApplicationModel> GetApplicationById(Guid id);
        public Task UpdateEnteranceStatus(Guid userId, EnteranceStatus newStatus);
        public Task DeleteApplicationById(Guid id);
    }
}
