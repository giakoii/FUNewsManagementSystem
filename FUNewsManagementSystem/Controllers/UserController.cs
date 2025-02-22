using System.Security.Claims;
using BusinessObject.SystemAccountService;
using FUNewsManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FUNewsManagementSystem.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="systemAccountService"></param>
        public UserController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        /// <summary>
        /// Get user index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (email == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // Get user
            var user = _systemAccountService.GetAccountProfileByEmail(email);

            var userViewModel = new UserProfileViewModel
            {
                Email = user.AccountEmail,
                Name = user.AccountName,
            };
            return View(userViewModel);
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update(UserProfileViewModel userProfile)
        {
            if (ModelState.IsValid)
            {
                var user = _systemAccountService.GetAccountByEmail(userProfile.Email);
                if (user != null)
                {
                    user.AccountName = userProfile.Name;
                    _systemAccountService.UpdateAccount(user);
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string email)
        {
            var userDelete = _systemAccountService.GetAccountByEmail(email);
            
            if (userDelete != null)
            {
                _systemAccountService.DeleteAccount(userDelete.AccountId);

                // Logout if user delete account
                if (User.Identity != null && User.Identity.Name == userDelete.AccountName)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("Index", "Home");
                }
            }
            
            return RedirectToAction("Index");
        }
    }
}
