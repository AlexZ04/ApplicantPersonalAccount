namespace ApplicantPersonalAccount.Common.Models.Enterance
{
    public class EducationProgramModel
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }
        public string EducationForm { get; set; }
        public FacultyModel Faculty { get; set; }
        public EducationLevelModel EducationLevel { get; set; }
    }
}
