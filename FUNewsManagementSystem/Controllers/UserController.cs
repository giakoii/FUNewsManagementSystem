using System.Security.Claims;
using BusinessObject.Service;
using FUNewsManagementSystem.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;

        /// <summary>
        /// Constructor
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
            var id = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Get user profile
            var user = _systemAccountService.GetById(id);
            var userViewModel = new UserProfileViewModel
            {
                Email = user.AccountEmail,
                Name = user.AccountName,
            };

            // Get news history
            var newsHistory = _systemAccountService.GetNewsHistory(id);

            var newsHistoryViewModel = newsHistory.Select(item => new NewsArticleHistoryViewModel
            {
                NewsArticleId = item.NewsArticleId,
                NewsTitle = item.NewsTitle,
                CreatedDate = item.CreatedDate,
                NewsContent = item.NewsContent,
                NewsSource = item.NewsSource,
                CategoryId = item.CategoryId,
            }).ToList();

            var viewModel = new UserProfileWithNewsHistoryViewModel
            {
                UserProfile = userViewModel,
                NewsArticleHistory = newsHistoryViewModel
            };

            return View(viewModel);
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
                if (short.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out short id))
                {
                    var user = _systemAccountService.GetById(id);
                    if (user != null)
                    {
                        user.AccountName = userProfile.Name;
                        _systemAccountService.UpdateUser(user);
                    }
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Error", "Home");
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
                _systemAccountService.DeleteSystemAccountAsync(userDelete.AccountId);

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
