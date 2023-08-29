using System.ComponentModel.DataAnnotations;

namespace Mag.Models.Dto.ForgetPassword
{
    public class ForgetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
