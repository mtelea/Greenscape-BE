using System.ComponentModel.DataAnnotations;

namespace Project1.Model
{
    public class ResetPasswordModel
    {

        [Required]
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }

    }
}
