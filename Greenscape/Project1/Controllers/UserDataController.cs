using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Dto;
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
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> SetUserProfilePicture(IFormFile picture)
        {

            if (picture == null || picture.Length == 0)
            {
                return BadRequest(new { Message = "Invalid picture file." });
            }

            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(username);
            var userId = await _userManager.GetUserIdAsync(user);
            var userData = await _context.UserData.FindAsync(userId);

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var fileExtension = Path.GetExtension(picture.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { Message = "Invalid file format. Only PNG, JPG, and JPEG files are allowed." });
            }

            if (user == null)
            {
                return NotFound(new { Message = "User was not found. Cannot set profile picture." });
            }

            if (userData == null)
            {
                UserData newUserData = new UserData();
                newUserData.UserID = userId;
                string newFileName = Guid.NewGuid().ToString();

                var filePath = Path.Combine("wwwroot/images/profilepictures", $"{newFileName}{Path.GetExtension(picture.FileName)}");
                var filePathToSave = Path.Combine("images/profilepictures", $"{newFileName}{Path.GetExtension(picture.FileName)}");

                using (var fileStream = System.IO.File.Create(filePath))
                {
                    await picture.CopyToAsync(fileStream);
                }

                newUserData.ProfilePicture = filePathToSave;
                _context.UserData.Add(newUserData);
            }

            else if (userData != null)
            {
                if (userData.ProfilePicture != null)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", userData.ProfilePicture);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                string newFileName = Guid.NewGuid().ToString();

                var filePath = Path.Combine("wwwroot/images/profilepictures", $"{newFileName}{Path.GetExtension(picture.FileName)}");
                var filePathToSave = Path.Combine("images/profilepictures", $"{newFileName}{Path.GetExtension(picture.FileName)}");

                using (var fileStream = System.IO.File.Create(filePath))
                {
                    await picture.CopyToAsync(fileStream);
                }
                userData.ProfilePicture = filePathToSave;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Profile picture set successfully." });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return BadRequest(new { Message = "Setting profile picture failed." });

            }

        }

        [HttpPost("get-user-profile-picture")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetUserProfilePicture()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(username);
            var userId = await _userManager.GetUserIdAsync(user);
            var userData = await _context.UserData.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new { Message = "User was not found. Cannot get profile picture." });
            }

            if (userData == null)
            {
                return NotFound(new { Message = "User has no profile picture" });
            }

            else if (userData != null)
            {
                if (userData.ProfilePicture == null)
                {
                    return NotFound(new { Message = "User has no profile picture." });
                }
            }
            return Ok(new { ProfilePicture = userData.ProfilePicture });
        }

        [HttpPost("update-user-points")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateUserPoints([FromBody]PointRequestModel pointRequestModel)
        {
            int points = pointRequestModel.Points;
            string operation = pointRequestModel.Operation;
            string source = pointRequestModel.Source;

            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(username);
            var userId = await _userManager.GetUserIdAsync(user);
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
                if (source.Contains("Course") == true || source.Contains("course") == true)
                {
                    try
                    {
                        var checkCourseHistory = await _context.PointsHistory
                            .Where(ph => ph.UserID == userId && ph.Source == source)
                            .FirstOrDefaultAsync();

                        if (checkCourseHistory != null)
                        {
                            return Ok(new { Message = "Course has already been bought." });
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                        return BadRequest(new { Message = "Error checking course points history." });
                    }
                }

                if (source.Contains("Daily") == true || source.Contains("daily") == true)
                {
                    try
                    {
                        DateTime twentyFourHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(24));

                        var checkDailyHistory = await _context.PointsHistory
                            .Where(ph => ph.UserID == userId && ph.Source == "Daily" && ph.EntryDate >= twentyFourHoursAgo)
                            .FirstOrDefaultAsync();

                        if (checkDailyHistory != null)
                        {
                            return BadRequest(new { Message = "Daily Check-In Bonus has already been claimed in the last 24h." });
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                        return BadRequest(new { Message = "Error checking daily points history." });
                    }
                }

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<UserDto>> GetUserData()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(username);
            var userId = await _userManager.GetUserIdAsync(user);
            var userData = await _context.UserData.FindAsync(userId);

            if (userData == null)
            {
                UserData newUserData = new UserData();
                newUserData.UserID = userId;
                newUserData.Points = 0;
                _context.UserData.Add(newUserData);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                    return BadRequest(new { Message = "Creating user data failed" });

                }
            }

            var userProfile = await _context.UserData
                .Include(u => u.ApplicationUser)
                .Where(u => u.UserID == userId)
                .Select(u => new UserDto
                {
                    UserID = u.UserID,
                    Username = u.ApplicationUser.UserName,
                    Email = u.ApplicationUser.Email,
                    Points = u.Points ?? 0,
                    ProfilePicture = u.ProfilePicture ?? "images/profilepictures\\\\default_profile_picture.jpg",
                })
                .FirstOrDefaultAsync();

            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

        // GET: api/UserData/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
