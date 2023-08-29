using System.ComponentModel.DataAnnotations;

namespace Mag.Models.Entities
{
    public class CategoryTag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public string? PicAddress { get; set; }
        public string? PicAlt { get; set; }
        public string? PicTitle { get; set; }
        public bool IsShowMainPage { get; set; }
        public bool IsActive { get; set; }
        public string? Type { get; set; }
    }
}
