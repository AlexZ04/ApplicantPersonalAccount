using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.Applicant;

namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IApplicantService
    {
        public Task SignToNotifications(string userEmail);
        public Task UnsignFromNotifications(string userEmail);
        public void EditInfoForEvents(EditApplicantInfoForEventsModel editedInfo, Guid userId);
        public Task<ApplicantInfoForEventsModel> GetInfoForEvents(Guid userId);
        public Task<bool> CanUserEdit(Guid userId);
    }
}
