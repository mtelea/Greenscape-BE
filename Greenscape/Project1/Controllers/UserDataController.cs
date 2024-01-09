using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Model;

namespace Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDataController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("add-user-profile-picture")]
        public async Task<IActionResult> AddUserProfilePicture(string userId, [FromBody] string pictureLink)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userData = await _context.UserData.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User was not found. Cannot update profile picture.");
            }

            if (userData == null)
            {
                UserData newUserData = new UserData();
                newUserData.UserID = userId;
                newUserData.ProfilePicture = pictureLink;
                _context.UserData.Add(newUserData);
            }

            else if (userData != null)
            {
                userData.ProfilePicture = pictureLink;
            }

            user.UserData.ProfilePicture = pictureLink;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               return BadRequest(new { Message = "Updating profile picture failed", Errors = e });

            }
            return Ok("Profile picture updated successfully.");

        }
/*
        [HttpPost("modify-user-points")]
        public async Task<IActionResult> ModifyUserPoints(string userId, [FromBody] int points, [FromBody] string operation)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userData = await _context.UserData.FindAsync(userId);

            if (operation == "add") { 

            }

            if (user == null)
            {
                return NotFound("User was not found. Cannot modify points.");
            }

            if (userData == null)
            {
                UserData newUserData = new UserData();
                newUserData.UserID = userId;
                newUserData.Points = 0;
                newUserData.Points += points;
                _context.UserData.Add(newUserData);
            }

            else if (userData != null)
            {
                userData.Points =;
            }

            user.UserData.ProfilePicture = pictureLink;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Updating profile picture failed", Errors = e });

            }
            return Ok("Profile picture updated successfully.");

        }*/

        // GET: api/UserData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserData>>> GetUserData()
        {
            return await _context.UserData.ToListAsync();
        }

        // GET: api/UserData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserData>> GetUserData(string id)
        {
            var userData = await _context.UserData.FindAsync(id);

            if (userData == null)
            {
                return NotFound();
            }

            return userData;
        }

        // PUT: api/UserData/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserData(string id, UserData userData)
        {
            if (id != userData.UserID)
            {
                return BadRequest();
            }

            _context.Entry(userData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserData
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserData>> PostUserData(UserData userData)
        {
            _context.UserData.Add(userData);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserDataExists(userData.UserID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserData", new { id = userData.UserID }, userData);
        }

        // DELETE: api/UserData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserData(string id)
        {
            var userData = await _context.UserData.FindAsync(id);
            if (userData == null)
            {
                return NotFound();
            }

            _context.UserData.Remove(userData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserDataExists(string id)
        {
            return _context.UserData.Any(e => e.UserID == id);
        }
    }
}
