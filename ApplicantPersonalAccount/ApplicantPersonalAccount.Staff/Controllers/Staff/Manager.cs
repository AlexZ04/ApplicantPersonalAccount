using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Staff.Domain.Infrascructure;
using ApplicantPersonalAccount.Staff.Domain.Services;
using ApplicantPersonalAccount.Staff.Models;
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
            var manager = await _serviceStorage.AdminManagerService.GetManagerProfile(id);

            return View(manager);
        }

        public IActionResult CreateManager()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveManagerInfo(ManagerProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View("ManagerInfo", model);

            //await _serviceStorage.AdminManagerService.UpdateManagerProfile(model);
            return RedirectToAction("WorkWithManagers");
        }

        [HttpPost]
        public IActionResult DeleteManager(Guid id)
        {
            _serviceStorage.AdminManagerService.DeleteManager(id);
            return RedirectToAction("WorkWithManagers");
        }
    }
}
