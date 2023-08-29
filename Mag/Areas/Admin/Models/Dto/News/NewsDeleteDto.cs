using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mag.Areas.Admin.Models.Dto.News
{
    public class NewsDeleteDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? IndexImageAddress { get; set; }
        public string? IndexImageAlt { get; set; }
        public string? IndexImageTitle { get; set; }
        public string? DescriptionHtmlEditor { get; set; }
        public string? KeyWords { get; set; }
        public List<int>? CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<int>? TagId { get; set; }
        public List<SelectListItem>? Tags { get; set; }
    }
}
