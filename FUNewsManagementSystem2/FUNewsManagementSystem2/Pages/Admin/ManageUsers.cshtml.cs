using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Service;
using BusinessObject.Enum;
using DataAccessObject.Models;
using FUNewsManagementSystem2.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem2.Pages.Admin;

/// <summary>
/// ManageUsers - PageModel for managing users
/// </summary>
[Authorize(Roles = ConstRole.Admin)]
public class ManageUsers : PageModel
{
    private readonly ISystemAccountService _systemAccountService;
    private readonly IMapper _mapper;
    
    [BindProperty] public SystemAccountViewModel UserView { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="systemAccountService"></param>
    /// <param name="mapper"></param>
    public ManageUsers(ISystemAccountService systemAccountService, IMapper mapper)
    {
        _systemAccountService = systemAccountService;
        _mapper = mapper;
    }

    public List<SystemAccountViewModel> Users { get; set; }

    /// <summary>
    /// Get users
    /// </summary>
    public async Task OnGetAsync()
    {
        var systemAccounts = await _systemAccountService.GetSystemAccountsAsync();
        Users = _mapper.Map<List<SystemAccountViewModel>>(systemAccounts);
    }

    /// <summary>
    /// Create user
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostCreateUserAsync()
    {
        if (!ModelState.IsValid)
        {
            // Check if model state is invalid
            var accounts = await _systemAccountService.GetSystemAccountsAsync();
            return Page();
        }

        // Convert to SystemAccount
        var account = _mapper.Map<SystemAccountDto>(UserView);
        var result = _systemAccountService.AddSystemAccount(account);
        
        if (!result)
        {
            ModelState.AddModelError(string.Empty, "Failed to create user.");
            var accounts = await _systemAccountService.GetSystemAccountsAsync();
            return Page();
        }

        return RedirectToPage();
    } 
    
    // /// <summary>
    // /// Delete user
    // /// </summary>
    // /// <param name="id"></param>
    // /// <returns></returns>
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
    /// <returns></returns>
    public async Task<IActionResult> OnPostUpdateUserAsync()
    {
        // Update user
        var userUpdate = await _systemAccountService.GetBy(acc => acc.AccountEmail == UserView.AccountEmail).FirstOrDefaultAsync();
        if (UserView.AccountEmail != null) userUpdate!.AccountEmail = UserView.AccountEmail;
        if (UserView.AccountName != null) userUpdate!.AccountName = UserView.AccountName;
        if (UserView.AccountRole != null) userUpdate!.AccountRole = UserView.AccountRole;
        
        await _systemAccountService.UpdateSystemAccountAsync(userUpdate);
        Users = _mapper.Map<List<SystemAccountViewModel>>(await _systemAccountService.GetSystemAccountsAsync());
        return RedirectToPage();
    }
}