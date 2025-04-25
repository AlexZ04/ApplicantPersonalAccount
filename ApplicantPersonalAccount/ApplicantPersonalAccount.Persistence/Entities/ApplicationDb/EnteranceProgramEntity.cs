namespace ApplicantPersonalAccount.Persistence.Entities.ApplicationDb
{
    public class EnteranceProgramEntity
    {
        public Guid Id { get; set; }
        public Guid ProgramId { get; set; }
        public int Priority { get; set; }
        public EnteranceEntity Enterance { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
