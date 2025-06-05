namespace ApplicantPersonalAccount.Common.Models.Enterance
{
    public class ApplicationModel
    {
        public Guid Id { get; set; }
        public EducationProgramModel Program { get; set; }
        public int Priority { get; set; }
    }
}
