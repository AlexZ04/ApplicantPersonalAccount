namespace ApplicantPersonalAccount.Common.DTOs
{
    public class GetProgramsDTO
    {
        public string Faculty { get; set; } = String.Empty;
        public string EducationForm { get; set; } = String.Empty;
        public string Language { get; set; } = String.Empty;
        public string Code { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
