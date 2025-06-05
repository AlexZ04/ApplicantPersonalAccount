namespace ApplicantPersonalAccount.Common.DTOs.Filters
{
    public class FilterByProgramDTO
    {
        public string Program { get; set; } = String.Empty;
        public List<string> Faculties { get; set; } = new List<string>();
    }
}
