using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Model;
using SendGrid.Helpers.Mail;
using System.Security.Claims;

namespace Project1.Controllers
{
    [Route("/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("get-all-users-roles")]
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

        [HttpGet("get-current-user-role")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetUserByEmail()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(username);
            var userId = await _userManager.GetUserIdAsync(user);

            if (username == null)
            {
                return BadRequest(new { Message = "No user found" });
            }

            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return Ok(userRoles);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserByEmail([FromQuery] string email)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var userToDelete = await _userManager.FindByEmailAsync(email);

            if (userToDelete == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var result = await _userManager.DeleteAsync(userToDelete);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User deleted successfully" });
            }

            return BadRequest(new { Message = "Failed to delete user", Errors = result.Errors });
        }

    }
}
