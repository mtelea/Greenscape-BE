using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project1.Model;
using System.Security.Claims;

namespace Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "Login successful" });
                }

                return BadRequest(new { Message = "Login failed", Errors = "Invalid login attempt" });
            }

            return BadRequest(new { Message = "Invalid login data" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (username == null)
            {
                return BadRequest(new { Message = "You are not logged in." });
            }

            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Signed out successfully" });
                
        }

    }
}
