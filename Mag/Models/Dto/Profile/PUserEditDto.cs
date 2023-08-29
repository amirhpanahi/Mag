using System.ComponentModel.DataAnnotations;

namespace Mag.Models.Dto.Profile
{
    public class PUserEditDto
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PicAddress { get; set; }
        public IFormFile? File { get; set; }
        [EmailAddress]
        public string? NewEmail { get; set; }
        [EmailAddress]
        public string? NewEmailConfirm { get; set; }




        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string? ConfirmNewPassword { get; set; }
    }
}
