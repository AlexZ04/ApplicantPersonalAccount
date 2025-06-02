using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.Staff.Domain.Services.Interfaces
{
    public interface IAdminDirectoryService
    {
        public Task<string> GetImportStatus();
        public void RequestImport(DirectoryImportType importType);
    }
}
