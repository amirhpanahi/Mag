using System.ComponentModel.DataAnnotations;

namespace Mag.Areas.Admin.Models.Dto.Tag
{
    public class TagAddDto
    {
        [Required]
        public string Name { get; set; }
        public string? Slug { get; set; }
        [Required]
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public string? PicAddress { get; set; }
        public string? PicAlt { get; set; }
        public string? PicTitle { get; set; }
        public IFormFile? File { get; set; }
        public bool IsShowMainPage { get; set; } = false;
        public bool IsActive { get; set; } = false;
    }
}
