using System.Security.Claims;
using BusinessObject.Service;
using BusinessObject.SystemAccountService;
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
        private readonly INewArticleService _newArticleService;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="systemAccountService"></param>
        public UserController(ISystemAccountService systemAccountService, INewArticleService newArticleService)
        {
            _systemAccountService = systemAccountService;
            _newArticleService = newArticleService;
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

            // Get user profile
            var user = _systemAccountService.GetAccountProfileByEmail(email);
            var userViewModel = new UserProfileViewModel
            {
                Email = user.AccountEmail,
                Name = user.AccountName,
            };

            // Get news history
            // Get news history
            var newsHistory = _systemAccountService.GetNewsHistory(email);

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
        
        [HttpGet]
        public IActionResult NewsHistory()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var newsHistory = _systemAccountService.GetNewsHistory(email);

            if (!newsHistory.Any())
            {
                return View(new List<NewsArticleHistoryViewModel>());
            }

            var newsHistoryViewModel = newsHistory.Select(item => new NewsArticleHistoryViewModel
            {
                
            }).ToList();

            return View(newsHistoryViewModel);
        }

    }
}
