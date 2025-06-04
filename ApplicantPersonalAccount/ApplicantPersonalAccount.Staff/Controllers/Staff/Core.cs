using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Staff.Domain.Infrascructure;
using ApplicantPersonalAccount.Staff.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.Staff.Controllers.Admin
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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> WorkWithDirectory()
        {
            ViewBag.ImportStatus = await _serviceStorage.AdminDirectoryService.GetImportStatus();
            ViewBag.ImportTypes = Enum.GetValues(typeof(DirectoryImportType))
                .Cast<DirectoryImportType>()
                .ToList();

            return View();
        }

        [HttpPost]
        public IActionResult ImportDirectory(DirectoryImportType importType)
        {
            _serviceStorage.AdminDirectoryService.RequestImport(importType);

            _logger.LogInformation($"Import of directory by type requested: {importType}");

            return RedirectToAction(nameof(WorkWithDirectory));
        }
    }
}
