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
        private readonly PointsHistory _pointsHistory;

        public UserDataController(AppDbContext context, UserManager<ApplicationUser> userManager, PointsHistory pointsHistory)
        {
            _context = context;
            _userManager = userManager;
            _pointsHistory = pointsHistory;
        }

        [HttpPost("set-user-profile-picture")]
        public async Task<IActionResult> SetUserProfilePicture(string userId, string pictureLink)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userData = await _context.UserData.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new { Message = "User was not found. Cannot set profile picture." });
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
                System.Diagnostics.Debug.WriteLine(e);
                return BadRequest(new { Message = "Setting profile picture failed." });

            }
            return Ok(new { Message = "Profile picture set successfully." });

        }

        [HttpPost("update-user-points")]
        public async Task<IActionResult> UpdateUserPoints(string userId, int points, string operation, string source)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userData = await _context.UserData.FindAsync(userId);
            var operationSign = 1;

            if (operation == "add")
            {
                operationSign = 1;
            } 
            else if (operation == "subtract")
            {
                operationSign = -1;
            }

            if (user == null)
            {
                return NotFound(new { Message = "User was not found. Cannot modify points."});
            }

            if (userData == null)
            {
                UserData newUserData = new UserData();
                PointsHistory pointsHistory = new PointsHistory();

                newUserData.UserID = userId;
                newUserData.Points = 0;
                if ((points * operationSign) < 0)
                {
                    return BadRequest(new { Message = "Points cannot go lower than 0", UserId = user.Id });
                }
                newUserData.Points += points * operationSign;

                pointsHistory.UserID = userId;
                pointsHistory.PointsModified = points * operationSign;
                pointsHistory.EntryDate = DateTime.Now;
                pointsHistory.Source = source;

                _context.UserData.Add(newUserData);
                _context.PointsHistory.Add(pointsHistory);
            }

            else if (userData != null)
            {
                if ((userData.Points + points * operationSign) < 0)
                {
                    return BadRequest(new { Message = "Points cannot go lower than 0", UserId = user.Id });
                }

                PointsHistory pointsHistory = new PointsHistory();
                pointsHistory.UserID = userId;
                pointsHistory.PointsModified = points * operationSign;
                pointsHistory.EntryDate = DateTime.Now;
                pointsHistory.Source = source;

                _context.PointsHistory.Add(pointsHistory);

                userData.Points += points * operationSign;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return BadRequest(new { Message = "Updating points failed, database transaction failed"});

            }
            return Ok(new { Message = "Points updated successfully.", UserId = user.Id });

        }

        [HttpPost("set-user-points")]
        public async Task<IActionResult> SetUserPoints(string userId, int points)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userData = await _context.UserData.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new { Message = "User was not found. Cannot set points." });
            }

            if (userData == null)
            {
                UserData newUserData = new UserData();
                newUserData.UserID = userId;
                newUserData.Points = 0;
                if (points < 0)
                {
                    return BadRequest(new { Message = "Points cannot be lower than zero" });
                }
                newUserData.Points = points;
                _context.UserData.Add(newUserData);
            }

            else if (userData != null)
            {
                if (points < 0)
                {
                    return BadRequest(new { Message = "Points cannot be lower than zero" });
                }
                userData.Points = points;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return BadRequest(new { Message = "Setting points failed"});

            }
            return Ok(new { Message = "Points set successfully." });

        }

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
