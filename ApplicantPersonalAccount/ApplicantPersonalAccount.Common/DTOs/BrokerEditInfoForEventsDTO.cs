using ApplicantPersonalAccount.Common.Models.Applicant;

namespace ApplicantPersonalAccount.Common.DTOs
{
    public class BrokerEditInfoForEventsDTO
    {
        Guid UserID { get; set; }
        EditApplicantInfoForEventsModel Model { get; set; }
    }
}
