using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;

namespace ApplicantPersonalAccount.Staff.Domain.Services
{
    public class ServiceStorage
    {
        public readonly IStaffAuthService StaffAuthService;
        public readonly IAdminDirectoryService AdminDirectoryService;
        public readonly IAdminManagerService AdminManagerService;

        public ServiceStorage(
            IStaffAuthService staffAuthService,
            IAdminDirectoryService adminDirectoryService,
            IAdminManagerService adminManagerService)
        {
            StaffAuthService = staffAuthService;
            AdminDirectoryService = adminDirectoryService;
            AdminManagerService = adminManagerService;
        }
    }
}
