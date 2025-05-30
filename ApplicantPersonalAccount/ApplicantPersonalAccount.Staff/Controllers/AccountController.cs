using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Staff.Domain.Services;
using ApplicantPersonalAccount.Staff.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.Staff.Controllers
{
    public class AccountController : Controller
    {
        private readonly ServiceStorage _serviceStorage;

        public AccountController(ServiceStorage serviceStorage)
        {
            _serviceStorage = serviceStorage;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            await _serviceStorage._staffAuthService.Logout();

            ViewBag.ReturnUrl = returnUrl;

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel, string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid) 
                return View(loginModel);

            if (! (await _serviceStorage._staffAuthService.Login(loginModel)))
                return Redirect(returnUrl ?? "/");

            ModelState.AddModelError(string.Empty, ErrorMessages.INVALID_CREDENTIALS);
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _serviceStorage._staffAuthService.Logout();

            return RedirectToAction("Action", "Home");
        }
    }
}
