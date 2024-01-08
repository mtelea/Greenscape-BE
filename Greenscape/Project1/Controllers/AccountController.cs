using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project1.Model;
using Project1.Service;

namespace Project1.Controllers
{
    [Route("/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            System.Diagnostics.Debug.WriteLine("finding email");

            if (user != null && user.Email != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("reset-password", "account", new { token, email = user.Email }, protocol: HttpContext.Request.Scheme);
                var emailSubject = "Reset Your Greenscape Password";
                var emailBody = $"Please reset the password of your Greenscape account by clicking <a href='{callbackUrl}'>here</a>.";

                await _emailSender.SendEmailAsync(user.Email, emailSubject, emailBody);

                return Ok(new { Message = "Password reset email sent successfully" });
            }

            return NotFound(new { Message = "User not found" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromQuery] string email, [FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Password reset successful" });
            }
            else
            {
                return BadRequest(new { Message = "Password reset failed", Errors = result.Errors });
            }
        }


    }
}
