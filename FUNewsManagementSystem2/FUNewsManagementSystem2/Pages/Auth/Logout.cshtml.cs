using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.Auth;

/// <summary>
/// Logout - PageModel for logging out
/// </summary>
public class Logout : PageModel
{
    /// <summary>
    /// Logout action
    /// </summary>
    public async Task OnGetAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Redirect("/Auth/Login");
    }
}