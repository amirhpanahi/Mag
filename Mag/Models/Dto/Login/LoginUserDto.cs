using System.ComponentModel.DataAnnotations;

namespace Mag.Models.Dto.Login
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="مرا به یاد بسپار")]
        public bool IsPersistent { get; set; }=false;
        public string ReturnUrl { get; set; } 
    }
}
