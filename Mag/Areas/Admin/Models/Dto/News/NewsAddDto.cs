using Mag.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mag.Areas.Admin.Models.Dto.News
{
    public class NewsAddDto
    {
        public string NewsSummary { get; set; }
        public string Title { get; set; }
        public string? Slug { get; set; }
        public string? DescriptionHtmlEditor { get; set; }
        public string? DescriptionSeo { get; set; }
        public string? KeyWords { get; set; }
        public string? VideoAddress { get; set; }
        public string? IndexImageAddress { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string? RegisterDatePersian { get; set; }
        public DateTime? DraftTime { get; set; }
        public string? DraftTimePersain { get; set; }
        public string? WriterId { get; set; }
        public bool IsActive { get; set; }
        public StatusName? Status { get; set; }
        public int? CountSeeNews { get; set; }
        public IFormFile? indexImageFile { get; set; }
        public IFormFile? VideoFile { get; set; }
        public List<int>? CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<int>? TagId { get; set; }
        public List<SelectListItem>? Tags { get; set; }
    }
}
