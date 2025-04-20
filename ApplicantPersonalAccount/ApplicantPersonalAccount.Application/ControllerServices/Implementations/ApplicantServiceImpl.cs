using ApplicantPersonalAccount.Application.ControllerServices;
using ApplicantPersonalAccount.Application.OuterServices;
using ApplicantPersonalAccount.Application.OuterServices.DTO;

namespace ApplicantPersonalAccount.Application.ControllerServices.Implementations
{
    public class ApplicantServiceImpl : IApplicantService
    {
        public IDirectoryService _directoryService;

        public ApplicantServiceImpl(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }

        public async Task<ProgramPagedList> GetListOfPrograms(
            string faculty, 
            string educationForm, 
            string language, 
            string code, 
            string name, 
            int page = 1, 
            int size = 5)
        {
            return await _directoryService.GetEducationPrograms(1, 10);
        }
    }
}
