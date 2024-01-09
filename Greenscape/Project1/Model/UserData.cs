using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project1.Model
{
    public class UserData
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public int? Points { get; set; } = 0;
        // TODO Add default profile picture
        public string? ProfilePicture {  get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
