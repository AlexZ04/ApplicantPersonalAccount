namespace ApplicantPersonalAccount.Application.OuterServices.DTO
{
    public class DocumentType
    {
        public string Name { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public List<EducationLevel> NextEducationLevels { get; set; }
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
