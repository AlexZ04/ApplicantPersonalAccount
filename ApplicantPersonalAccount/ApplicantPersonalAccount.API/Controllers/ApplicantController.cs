using ApplicantPersonalAccount.Application.ControllerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.API.Controllers
{
    [Route("api/appliant")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;

        public ApplicantController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }

        [HttpGet("programs")]
        public async Task<IActionResult> GetListOfPrograms(
            [FromQuery] string faculty, 
            [FromQuery] string educationForm,
            [FromQuery] string language,
            [FromQuery] string code,
            [FromQuery] string name,
            [FromQuery] int page = 1,
            [FromQuery] int size = 5)
        {
            return Ok(await _applicantService.GetListOfPrograms(faculty, educationForm, language, code, name, page, size));
        }
    }
}
