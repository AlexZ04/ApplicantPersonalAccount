using ApplicantPersonalAccount.Common.Models.Applicant;

namespace ApplicantPersonalAccount.Common.DTOs
{
    public class BrokerEditInfoForEventsDTO
    {
        public Guid UserId { get; set; }
        public EditApplicantInfoForEventsModel Model { get; set; }
    }
}
