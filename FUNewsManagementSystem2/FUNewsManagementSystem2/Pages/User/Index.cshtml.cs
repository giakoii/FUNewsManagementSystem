using System.Security.Claims;
using AutoMapper;
using BusinessLogic.Service;
using FUNewsManagementSystem.Models.ViewModel;
using FUNewsManagementSystem2.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.User;

public class Index : PageModel
{
    private readonly ISystemAccountService _systemAccountService;
    private readonly IMapper _mapper;

    [BindProperty] public UserProfileWithNewsHistoryViewModel UserProfile { get; set; } = new();

    public Index(ISystemAccountService systemAccountService, IMapper mapper)
    {
        _systemAccountService = systemAccountService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get user profile
    /// </summary>
    /// <returns></returns>
    public IActionResult OnGet()
    {
        if (!short.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out short id))
        {
            TempData["ToastMessage"] = "Invalid user ID!";
            TempData["ToastType"] = "danger";
            return RedirectToPage("/Index");
        }

        var user = _systemAccountService.GetById(id);
        if (user == null)
        {
            TempData["ToastMessage"] = "User not found!";
            TempData["ToastType"] = "danger";
            return RedirectToPage("/Index");
        }

        // Get user profile and news his/her history
        UserProfile = new UserProfileWithNewsHistoryViewModel
        {
            UserProfile = _mapper.Map<UserProfileViewModel>(user) ?? new UserProfileViewModel(),
            NewsArticleHistory = _mapper.Map<List<NewsArticleHistoryViewModel>>(
                _systemAccountService.GetNewsHistoryByAccountId(id)) ?? new List<NewsArticleHistoryViewModel>()
        };

        return Page();
    }
    
    /// <summary>
    /// Update user profile
    /// </summary>
    /// <returns></returns>
    public IActionResult OnPostUpdate()
    {
        if (!ModelState.IsValid)
        {
            TempData["ToastMessage"] = "Invalid data!";
            TempData["ToastType"] = "danger";
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Field: {entry.Key}, Error: {error.ErrorMessage}");
                }
            }

            return Page(); 
        }

        if (!short.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out short id))
        {
            TempData["ToastMessage"] = "Invalid user ID!";
            TempData["ToastType"] = "danger";
            return RedirectToPage("/Index");
        }

        var user = _systemAccountService.GetById(id);
        if (user == null)
        {
            TempData["ToastMessage"] = "User not found!";
            TempData["ToastType"] = "danger";
            return RedirectToPage("/Index");
        }

        var existingUser = _systemAccountService.GetAccountByEmail(UserProfile.UserProfile.Email);
        if (existingUser != null && existingUser.AccountId != id)
        {
            return RedirectToPage();
        }

        user.AccountName = UserProfile.UserProfile.Name;
        user.AccountEmail = UserProfile.UserProfile.Email;
        _systemAccountService.UpdateUser(user);

        TempData["ToastMessage"] = "Profile updated successfully!";
        TempData["ToastType"] = "success";
        
        return RedirectToPage();
    }
    
    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostDelete(string email)
    {
        if (!short.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out short id))
        {
            TempData["ToastMessage"] = "Invalid user ID!";
            TempData["ToastType"] = "danger";
            return RedirectToPage("/Index");
        }

        var userDelete = _systemAccountService.GetById(id);

        if (userDelete != null)
        {
            await _systemAccountService.DeleteSystemAccountAsync(userDelete.AccountId);

            if (User.Identity != null && User.Identity.Name == userDelete.AccountName)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToPage("/Auth/Login");
            }
        }

        return RedirectToPage();
    }

}