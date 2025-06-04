using ApplicantPersonalAccount.Common.Constants;
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
            var newManager = new ManagerCreateModel();

            return View(newManager);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewManager(ManagerCreateModel model)
        {
            if (!ModelState.IsValid)
                return View("CreateManager", model);

            var isCreated = await _serviceStorage.AdminManagerService.CreateManager(model);

            if (isCreated)
                return RedirectToAction("WorkWithManagers");

            ModelState.AddModelError(string.Empty, ErrorMessages.CANT_REGISTER_USER);

            return View("CreateManager", model);
        }

        [HttpPost]
        public IActionResult EditManagerInfo(ManagerProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _serviceStorage.AdminManagerService.EditManagerProfile(model);
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
