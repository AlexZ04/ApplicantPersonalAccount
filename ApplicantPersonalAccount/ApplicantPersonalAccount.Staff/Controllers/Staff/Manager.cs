using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Staff.Domain.Infrascructure;
using ApplicantPersonalAccount.Staff.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.Staff.Controllers.Staff
{
    [TokenAuthFilter]
    [Authorize(Roles = "Admin")]
    [CheckToken]
    public partial class StaffController : Controller
    {
        private readonly ServiceStorage _serviceStorage;

        public StaffController(ServiceStorage serviceStorage)
        {
            _serviceStorage = serviceStorage;
        }

        public async Task<IActionResult> WorkWithManagers()
        {
            ViewBag.Managers = await _serviceStorage.AdminManagerService.GetListOfManagers();

            return View();
        }

        public async Task<IActionResult> ManagerInfo(Guid id)
        {
            return View();
        }

        public IActionResult CreateManager()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteManager(Guid id)
        {
            //await _serviceStorage.AdminManagerService.DeleteManager(id);
            return RedirectToAction("WorkWithManagers");
        }
    }
}
