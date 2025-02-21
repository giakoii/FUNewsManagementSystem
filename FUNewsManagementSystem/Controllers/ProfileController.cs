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
        private readonly IAuthService _authService;

        public ProfileController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            var userId = short.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _authService.GetUserById(userId);
            return View(user);
        }

        [HttpPost]
        public IActionResult Update(SystemAccount user)
        {
            // Validation và update logic
            _authService.UpdateUser(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
