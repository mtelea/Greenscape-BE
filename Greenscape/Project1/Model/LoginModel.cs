using System.ComponentModel.DataAnnotations;

namespace Project1.Model
{
    public class LoginModel
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
