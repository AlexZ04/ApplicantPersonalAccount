using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;

namespace ApplicantPersonalAccount.Staff.Domain.Services
{
    public class ServiceStorage
    {
        private readonly IStaffAuthService _staffAuthService;

        public ServiceStorage(IStaffAuthService staffAuthService)
        {
            _staffAuthService = staffAuthService;
        }
    }
}
