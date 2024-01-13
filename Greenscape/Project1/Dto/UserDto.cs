using Microsoft.AspNetCore.Mvc;

namespace Project1.Dto
{
    public class UserDto
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public int? Points { get; set; } = 0;
        public string Email { get; set; }
        // TODO Add default profile picture
        public string? ProfilePicture { get; set; }
    }
}
