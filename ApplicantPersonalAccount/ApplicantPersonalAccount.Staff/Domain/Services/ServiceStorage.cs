using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;

namespace ApplicantPersonalAccount.Staff.Domain.Services
{
    public class ServiceStorage
    {
        public readonly IStaffAuthService _staffAuthService;

        public ServiceStorage(IStaffAuthService staffAuthService)
        {
            _staffAuthService = staffAuthService;
        }
    }
}
