using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.UserAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.UserAuth.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public StaffController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet("managers")]
        [Authorize(Roles = "HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> GetManagersList()
        {
            return Ok(await _managerService.GetAllManagers());
        }
    }
}
