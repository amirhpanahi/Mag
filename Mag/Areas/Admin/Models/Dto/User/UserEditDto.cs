using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Mag.Areas.Admin.Models.Dto.User
{
    public class UserEditDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? PicAddress { get; set; }
        public string? PicTitle { get; set; }
        public string? PicAlt { get; set; }
        public IFormFile? File { get; set; }
    }
}
