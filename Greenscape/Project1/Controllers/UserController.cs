﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Project1.Controllers
{
    [Route("/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("get-users-roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }
            var users = await _userManager.Users.ToListAsync();
            var usersWithRoles = new List<object>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = userRoles
                });
            }

            return Ok(usersWithRoles);
        }

        [HttpGet("get-role-by-email")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Message = "Email cannot be empty." });
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var userWithRoles = new
            {
                UserName = user.UserName,
                Email = user.Email,
                Roles = userRoles
            };

            return Ok(userWithRoles);
        }
    }
}
