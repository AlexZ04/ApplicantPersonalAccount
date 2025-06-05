using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.User;

namespace ApplicantPersonalAccount.Common.Models.Enterance
{
    public class EnteranceModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public UserProfileModel Applicant { get; set; } = new UserProfileModel();
        public List<ApplicationModel> Programs { get; set; } = new List<ApplicationModel>();
        public ManagerModel? Manager { get; set; }
        public EnteranceStatus Status { get; set; } = EnteranceStatus.Created;
        public DateTime? UpdateTime { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
