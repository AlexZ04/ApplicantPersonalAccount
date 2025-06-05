namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IStaffService
    {
        public Task AttachEnteranceToManager(Guid userId, Guid managerId, bool sendEmail = true);
        public Task UnattachEnteranceFromManager(Guid userId);
        
    }
}
