using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Staff.Domain.Infrascructure;
using ApplicantPersonalAccount.Staff.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.Staff.Controllers.Admin
{
    //[TokenAuthFilter]
    //[Authorize(Roles = "Admin")]
    //[CheckToken]
    public partial class StaffController : Controller
    {
        private readonly ServiceStorage _serviceStorage;

        public StaffController(ServiceStorage serviceStorage)
        {
            _serviceStorage = serviceStorage;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WorkWithDirectory()
        {
            return View();
        }

        public IActionResult WorkWithManagers()
        {
            return View();
        }
    }
}
