namespace ApplicantPersonalAccount.UserAuth.Services
{
    public interface IFilterService
    {
        public Task<List<Guid>> GetFilteredIds(string name);
    }
}
