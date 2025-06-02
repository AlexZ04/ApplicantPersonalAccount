using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;

namespace ApplicantPersonalAccount.Staff.Domain.Services
{
    public class ServiceStorage
    {
        public readonly IStaffAuthService StaffAuthService;
        public readonly IAdminDirectoryService AdminDirectoryService;

        public ServiceStorage(
            IStaffAuthService staffAuthService,
            IAdminDirectoryService adminDirectoryService)
        {
            StaffAuthService = staffAuthService;
            AdminDirectoryService = adminDirectoryService;
        }
    }
}
