﻿using BusinessObject.Service;
using DataAccessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;

        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = await _accountService.GetSystemAccountsAsync();
            return View("_ManageUsers", users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] SystemAccount account)
        {
            if (account == null)
                return BadRequest("Invalid account data.");

            var result = await _accountService.CreateSystemAccountAsync(account);
            if (!result)
                return BadRequest("Failed to create user.");
            return Ok("User created successfully.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(short id)
        {
            var result = await _accountService.DeleteSystemAccountAsync(id);
            if (!result)
                return BadRequest("Failed to delete user.");

            return Ok("User deleted successfully.");
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] SystemAccount account)
        {
            if (account == null)
                return BadRequest("Invalid account data.");

            await _accountService.UpdateSystemAccountAsync(account);
            return Ok("User updated successfully.");
        }
    }
}
