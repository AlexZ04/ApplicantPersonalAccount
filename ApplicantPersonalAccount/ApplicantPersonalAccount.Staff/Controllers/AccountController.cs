﻿using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Infrastructure.Utilities;
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
        public IActionResult Login(string? returnUrl)
        {
            _serviceStorage.StaffAuthService.DeleteCookies();

            ViewBag.ReturnUrl = returnUrl;

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(loginModel);
            }

            if (await _serviceStorage.StaffAuthService.Login(loginModel))
                return Redirect(returnUrl ?? "/");

            ModelState.AddModelError(string.Empty, ErrorMessages.INVALID_CREDENTIALS);
            ViewBag.ReturnUrl = returnUrl;
            return View(loginModel);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _serviceStorage.StaffAuthService.Logout(UserDescriptor.GetUserId(User));
            return RedirectToAction("Login", "Account");
        }
    }
}
