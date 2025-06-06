using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Common.Models.Enterance;

namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IEnteranceService
    {
        public Task<EnteranceModel> GetEnteranceByUserId(Guid userId);
        public Task<ApplicationModel> GetApplicationById(Guid id);
        public Task UpdateEnteranceStatus(Guid userId, EnteranceStatus newStatus);
        public Task DeleteApplicationById(Guid id);
        public Task EditAppicationById(Guid id, EducationProgramApplicationModel applicationModel, 
            string actingUser, string userRole, Guid userId);
        public Task<EnterancePagedListModel> GetEnterances(
            string? name,
            string? program,
            List<string>? faculties,
            EnteranceStatus? status,
            bool hasManagerOnly,
            bool attachedToManager,
            SortingType? sortedByUpdateDate,
            Guid managerId,
            int page = 1,
            int size = 5);

        public Task CreateEnteranceForUser(Guid userId);
    }
}
