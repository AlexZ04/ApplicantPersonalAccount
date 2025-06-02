namespace ApplicantPersonalAccount.Staff.Domain.Services.Interfaces
{
    public interface IAdminDirectoryService
    {
        public Task<string> GetImportStatus();
    }
}
