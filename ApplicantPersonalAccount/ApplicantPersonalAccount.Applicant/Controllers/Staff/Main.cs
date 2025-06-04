using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.Applicant.Controllers.Staff
{
    [Route("api/staff")]
    [ApiController]
    public partial class StaffController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult BB()
        {
            return Ok();
        }
    }
}
