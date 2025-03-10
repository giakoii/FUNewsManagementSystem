using System.Diagnostics;
using System.Security.Claims;
using BusinessObject.Enum;
using BusinessObject.Service;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Controllers;

public class AuthenController : Controller
{
    private readonly ISystemAccountService _systemAccountService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="systemAccountService"></param>
    public AuthenController(ISystemAccountService systemAccountService)
    {
        _systemAccountService = systemAccountService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "NewArticle");
        }

        return View(new LoginRequest());
    }
    
    /// <summary>
    /// Login action (POST)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", request);
        }

        var user = await _systemAccountService.LoginAsync(request.Email, request.Password);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            user = await _systemAccountService.LoginAdmin(request.Email, request.Password);
            if (user == null)
                return View("Index", request);
        }

        string role = "";

        if (user.AccountRole == (int)EnumRole.Staff)
        {
            role = "Staff";
        }
        if (user.AccountRole == (int)EnumRole.Lecturer)
        {
            role = "Lecturer";
        }
        if (user.AccountRole == (int)EnumRole.Admin)
        {
            role = "Admin";
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
            new Claim(ClaimTypes.Name, user.AccountName!),
            new Claim(ClaimTypes.Email, user.AccountEmail!),
            new Claim(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
        new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddDays(7)
        });

        return (role == "Admin") ? RedirectToAction("Dashboard", "Admin") : RedirectToAction("", "NewArticle");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "NewArticle");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}