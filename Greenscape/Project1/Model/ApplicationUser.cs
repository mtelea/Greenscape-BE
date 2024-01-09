using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Project1.Model
{
    public class ApplicationUser : IdentityUser
    {
        public UserData UserData { get; set; }
        public ICollection<PointsHistory> PointsHistory { get; set; }
    }
}
