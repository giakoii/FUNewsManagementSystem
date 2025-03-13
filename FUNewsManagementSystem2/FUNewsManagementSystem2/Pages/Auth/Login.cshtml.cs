using System.Security.Claims;
using BusinessObject.Enum;
using BusinessObject.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.Auth;

public class Login : PageModel
{
    private readonly ISystemAccountService _systemAccountService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="systemAccountService"></param>
    public Login(ISystemAccountService systemAccountService)
    {
        _systemAccountService = systemAccountService;
    }

    [BindProperty] 
    public LoginRequest LoginRequest { get; set; } = new LoginRequest
    {
        Email = string.Empty,
        Password = string.Empty
    };
    
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            Response.Redirect("/NewArticle");
        }
    }

    /// <summary>
    /// Login action (POST)
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostAsync()
    {
        // Check if model state is valid
        if (!ModelState.IsValid) 
            return Page();

        // Login
        var user = await _systemAccountService.LoginAsync(LoginRequest.Email, LoginRequest.Password) ??
                   
                   await _systemAccountService.LoginAdmin(LoginRequest.Email, LoginRequest.Password);

        // Check if user is null
        if (user == null)
        {
            ErrorMessage = "Invalid email or password";
            return Page();
        }

        // Get role
        var role = user.AccountRole switch
        {
            (int)EnumRole.Staff => "Staff",
            (int)EnumRole.Lecturer => "Lecturer",
            (int)EnumRole.Admin => "Admin",
            _ => "User"
        };

        // Create claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
            new(ClaimTypes.Name, user.AccountName!),
            new(ClaimTypes.Email, user.AccountEmail!),
            new(ClaimTypes.Role, role)
        };

        // Create identity
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddDays(7) });

        return RedirectToPage(role == "Admin" ? "/Admin/Dashboard" : "/NewArticle/Index");
    }
}