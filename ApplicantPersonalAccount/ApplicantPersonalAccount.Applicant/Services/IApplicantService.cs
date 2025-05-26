using ApplicantPersonalAccount.Common.Models.Applicant;

namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IApplicantService
    {
        public Task SignToNotifications(string userEmail);
        public Task UnsignFromNotifications(string userEmail);
        public Task EditInfoForEvents(EditApplicantInfoForEventsModel editedInfo, Guid userId);
        public Task<ApplicantInfoForEventsModel> GetInfoForEvents(Guid userId);
    }
}
