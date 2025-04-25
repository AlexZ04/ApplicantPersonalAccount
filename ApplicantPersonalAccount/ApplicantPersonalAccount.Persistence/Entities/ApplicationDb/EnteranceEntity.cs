using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.Persistence.Entities.ApplicationDb
{
    public class EnteranceEntity
    {
        public Guid Id { get; set; }
        public Guid ApplicantId { get; set; }
        public Guid? ManagerId { get; set; }
        public EnteranceStatus Status { get; set; }
        public List<EnteranceProgramEntity> Programs {  get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
