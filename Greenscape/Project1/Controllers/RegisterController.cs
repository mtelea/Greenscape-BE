﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project1.Model;
using System.Threading.Tasks;

namespace Project1.Controllers
{
    [Route("/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // Check if roles exist, if not, create them
            string[] roleNames = { "Admin", "Manager", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign "User" role to the newly registered user
                    await _userManager.AddToRoleAsync(user, "User");

                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return Ok(new { Message = "Registration successful", UserId = user.Id });
                }

                return BadRequest(new { Message = "Registration failed", Errors = result.Errors });
            }

            return BadRequest(new { Message = "Invalid registration data" });
        }

        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdminOnlyIfAdmin([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");

                    return Ok(new { Message = "Registration successful", UserId = user.Id });
                }

                return BadRequest(new { Message = "Registration failed", Errors = result.Errors });
            }

            return BadRequest(new { Message = "Invalid registration data" });
        }

        [HttpPost("register-manager")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RegisterManagerOnlyIfManager([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Manager");

                    return Ok(new { Message = "Registration successful", UserId = user.Id });
                }

                return BadRequest(new { Message = "Registration failed", Errors = result.Errors });
            }

            return BadRequest(new { Message = "Invalid registration data" });
        }

    }
}
