using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Mag.Models.Dto.Register
{
    public class RegisterUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public string? PicTitle { get; set; }
        public string? PicAlt { get; set; }

        public IFormFile? File { get; set; }

    }
}
