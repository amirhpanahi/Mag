using Microsoft.AspNetCore.Identity;

namespace Mag.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateRegister { get; set; }
        public string? DateRegisterPresian { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string? LastLoginDatePersian { get; set; }
        public string? PicAddress { get; set; }
        public string? PicAlt { get; set; }
        public string? PicTitle { get; set; }
    }
}
