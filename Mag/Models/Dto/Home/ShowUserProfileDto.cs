using Microsoft.AspNetCore.Http;

namespace Mag.Models.Dto.Home
{
    public class ShowUserProfileDto
    {
        public string? BannerForProfile { get; set; }
        public string? AboutMe { get; set; }

        public string? FullName { get; set; }
        public string? RegisterDate { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserPicture { get; set; }
        public string? UserPictureAlt { get; set; }
        public string? UserPictureTitle { get; set; }

        public IFormFile? BannerPicFile { get; set; }
    }
}
