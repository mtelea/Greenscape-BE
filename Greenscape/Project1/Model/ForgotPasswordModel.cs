using System.ComponentModel.DataAnnotations;

namespace Project1.Model
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress] public required string Email { get; set; }
    }
}
