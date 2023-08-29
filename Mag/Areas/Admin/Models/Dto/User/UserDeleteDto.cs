using System.ComponentModel.DataAnnotations;

namespace Mag.Areas.Admin.Models.Dto.User
{
    public class UserDeleteDto
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? PicAddress { get; set; }
        public string? PicTitle { get; set; }
        public string? PicAlt { get; set; }
    }
}
