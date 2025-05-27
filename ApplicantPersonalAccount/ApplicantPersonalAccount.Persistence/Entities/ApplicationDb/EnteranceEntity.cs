using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.Persistence.Entities.ApplicationDb
{
    public class EnteranceEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ApplicantId { get; set; }
        public Guid? ManagerId { get; set; } = null;
        public EnteranceStatus Status { get; set; } = EnteranceStatus.Created;
        public List<EnteranceProgramEntity> Programs { get; set; } = new List<EnteranceProgramEntity>();
        public DateTime? UpdateTime { get; set; } = null;
    }
}
