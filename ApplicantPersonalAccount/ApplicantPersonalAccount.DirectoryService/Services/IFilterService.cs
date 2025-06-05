namespace ApplicantPersonalAccount.DirectoryService.Services
{
    public interface IFilterService
    {
        public Task<List<Guid>> GetFilteredPrograms(string program, List<string> faculties);
    }
}
