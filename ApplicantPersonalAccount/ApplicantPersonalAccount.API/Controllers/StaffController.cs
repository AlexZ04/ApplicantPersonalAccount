using ApplicantPersonalAccount.Application.ControllerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.API.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController: ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }
    }
}
