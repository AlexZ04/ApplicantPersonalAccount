
using ApplicantPersonalAccount.Persistence.Contextes;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class StaffServiceImpl : IStaffService
    {
        private readonly ApplicationDataContext _applicationContext;
        private readonly ILogger<StaffServiceImpl> _logger;

        public StaffServiceImpl(
            ApplicationDataContext applicationContext, 
            ILogger<StaffServiceImpl> logger)
        {
            _applicationContext = applicationContext;
            _logger = logger;
        }

        public Task AttachEnteranceToManager(Guid userId, Guid managerId)
        {
            throw new NotImplementedException();
        }

        public Task UnattachEnteranceFromManager(Guid userId, Guid managerId)
        {
            throw new NotImplementedException();
        }
    }
}
