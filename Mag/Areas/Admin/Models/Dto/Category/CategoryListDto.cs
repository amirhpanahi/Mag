using System.ComponentModel.DataAnnotations;

namespace Mag.Areas.Admin.Models.Dto.Category
{
    public class CategoryListDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public Dictionary<int,string>? NameParentId { get; set; }
        public string? PicAddress { get; set; }
        public string? PicAlt { get; set; }
        public string? PicTitle { get; set; }

        [Display(Name = "نمایش در صفحه اصلی")]
        public bool IsShowMainPage { get; set; }
        [Display(Name = "فعال بودن")]
        public bool IsActive { get; set; }
    }
}
