namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IStaffService
    {
        public Task AttachEnteranceToManager(Guid userId, Guid managerId);
        public Task UnattachEnteranceFromManager(Guid userId);
        
    }
}
