namespace Mag.Areas.Admin.Models.Dto.Tag
{
    public class TagListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public string? PicAddress { get; set; }
        public string? PicAlt { get; set; }
        public string? PicTitle { get; set; }
        public bool IsShowMainPage { get; set; }=false;
        public bool IsActive { get; set; } = false;
    }
}
