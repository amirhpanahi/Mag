using Mag.Models.Entities;

namespace Mag.Areas.Admin.Models.Dto.News
{
    public class NewsCardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Slug { get; set; }
        public string? DescriptionHtmlEditor { get; set; }
        public string? DescriptionSeo { get; set; }
        public string? KeyWords { get; set; }
        public string? Categories { get; set; }
        public string? Tags { get; set; }
        public string? VideoAddress { get; set; }
        public string? IndexImageAddress { get; set; }
        public string? RegisterDatePersian { get; set; }
        public string? DraftTimePersain { get; set; }
        public string? PublishNewsDatePersianDay { get; set; }
        public string? PublishNewsDatePersianmonth { get; set; }
        public string? PublishNewsDatePersianYear { get; set; }
        public string? PublishNewsDatePersianTime { get; set; }
        public string? UserImage { get; set; }
        public string? UserFullName { get; set; }
        public string? WriterId { get; set; }
        public bool IsActive { get; set; }
        public StatusName? Status { get; set; }
        public int? CountSeeNews { get; set; }
        public string? IndexImageAddressAlt { get; set; }
        public string? IndexImageAddressTitle { get; set; }
    }
}
