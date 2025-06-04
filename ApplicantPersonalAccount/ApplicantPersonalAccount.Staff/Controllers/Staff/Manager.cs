using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs.Managers;
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
        private readonly ILogger<StaffController> _logger;

        public StaffController(
            ServiceStorage serviceStorage,
            ILogger<StaffController> logger)
        {
            _serviceStorage = serviceStorage;
            _logger = logger;
        }

        public async Task<IActionResult> WorkWithManagers()
        {
            try
            {
                ViewBag.Managers = await _serviceStorage.AdminManagerService.GetListOfManagers();
                _logger.LogInformation($"List of managers loaded");
            }
            catch (Exception ex)
            {
                ViewBag.Managers = new List<ManagerDTO>();
                ModelState.AddModelError(string.Empty, ex.Message);
                _logger.LogWarning(ex.Message);
            }
            return View();
        }

        public async Task<IActionResult> ManagerInfo(Guid id)
        {
            try
            {
                var manager = await _serviceStorage.AdminManagerService.GetManagerProfile(id);
                _logger.LogWarning($"Profile loaded, Manager id: {id}");
                return View(manager);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                _logger.LogWarning(ex.Message);
                return View("WorkWithManagers");
            }
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

            try
            {
                var isCreated = await _serviceStorage.AdminManagerService.CreateManager(model);

                if (isCreated)
                {
                    _logger.LogInformation($"User {model.Email} created");
                    return RedirectToAction("WorkWithManagers");
                }
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                _logger.LogWarning(ex.Message);
            }

            ModelState.AddModelError(string.Empty, ErrorMessages.CANT_REGISTER_USER);
            _logger.LogError(ErrorMessages.CANT_REGISTER_USER + $" User email: {model.Email}");

            return View("CreateManager", model);
        }

        [HttpPost]
        public IActionResult EditManagerInfo(ManagerProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _serviceStorage.AdminManagerService.EditManagerProfile(model);
            _logger.LogInformation($"Trying to edit {model.Email} user profile");
            return RedirectToAction("WorkWithManagers");
        }

        [HttpPost]
        public IActionResult DeleteManager(Guid id)
        {
            _serviceStorage.AdminManagerService.DeleteManager(id);
            _logger.LogInformation($"Trying to delete {id} user profile");
            return RedirectToAction("WorkWithManagers");
        }
    }
}
