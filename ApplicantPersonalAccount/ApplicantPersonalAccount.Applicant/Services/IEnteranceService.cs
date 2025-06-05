using ApplicantPersonalAccount.Common.Models.Enterance;

namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IEnteranceService
    {
        public Task<EnteranceModel> GetEnteranceByUserId(Guid userId);
        public Task<ApplicationModel> GetApplicationById(Guid id);
    }
}
