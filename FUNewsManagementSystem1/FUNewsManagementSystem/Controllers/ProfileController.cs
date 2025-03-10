using BusinessObject.Service;
using DataAccessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    [Authorize(Roles = "Staff")]
    public class ProfileController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;

        public ProfileController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public IActionResult Index()
        {
            var userId = short.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _systemAccountService.GetById(userId);
            return View(user);
        }

        [HttpPost]
        public IActionResult Update(SystemAccount user)
        {
            // Validation và update logic
            _systemAccountService.UpdateUser(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
