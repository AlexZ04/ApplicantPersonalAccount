using ApplicantPersonalAccount.Common.Models;

namespace ApplicantPersonalAccount.Application.OuterServices.DTO
{
    public class ProgramPagedList
    {
        public List<EducationProgram> Programs { get; set; }
        public PageInfoModel Pagination { get; set; }
    }
}
