using BusinessObject.Enum;
using BusinessObject.Service;
using DataAccessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.Admin;

/// <summary>
/// ManageUsers - PageModel for managing users
/// </summary>
[Authorize(Roles = ConstRole.Admin)]
public class ManageUsers : PageModel
{
    private readonly ISystemAccountService _systemAccountService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="systemAccountService"></param>
    public ManageUsers(ISystemAccountService systemAccountService)
    {
        _systemAccountService = systemAccountService;
    }

    public List<SystemAccount> Users { get; set; }

    /// <summary>
    /// Get users
    /// </summary>
    public async Task OnGetAsync()
    {
        Users = await _systemAccountService.GetSystemAccountsAsync();
    }

    /// <summary>
    /// Create user
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostCreateUserAsync([FromBody] SystemAccount account)
    {
        // Check if account is null
        if (account == null) 
            return BadRequest("Invalid account data.");

        // Create user
        var result = await _systemAccountService.CreateSystemAccountAsync(account);
        return result ? new JsonResult("User created successfully.") : BadRequest("Failed to create user.");
    }

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnDeleteAsync(short id)
    {
        // Delete user
        var result = await _systemAccountService.DeleteSystemAccountAsync(id);
        
        // Return result
        return result ? new JsonResult("User deleted successfully.") : BadRequest("Failed to delete user.");
    }

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPutUpdateUserAsync([FromBody] SystemAccount account)
    {
        // Check if account is null
        if (account == null) 
            return BadRequest("Invalid account data.");

        // Update user
        await _systemAccountService.UpdateSystemAccountAsync(account);
        return new JsonResult("User updated successfully.");
    }
}