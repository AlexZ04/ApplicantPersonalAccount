using ApplicantPersonalAccount.Application.OuterServices.DTO;

namespace ApplicantPersonalAccount.Application.ControllerServices
{
    public interface IApplicantService
    {
        public Task<ProgramPagedList> GetListOfPrograms(
            string faculty,
            string educationForm,
            string language,
            string code,
            string name,
            int page = 1,
            int size = 5);
    }
}
