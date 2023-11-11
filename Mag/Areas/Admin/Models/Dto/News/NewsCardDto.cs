using Mag.Areas.Admin.Models.Dto.Comment;
using Mag.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Mag.Areas.Admin.Models.Dto.News
{
    public class NewsCardDto
    {
        public int Id { get; set; }
        public string? NewsSummary { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? DescriptionHtmlEditor { get; set; }
        public string? DescriptionSeo { get; set; }
        public string? LikeStatus { get; set; }
        public int? CountOfLike { get; set; }
        public int? CountOfComment { get; set; }
        public string? KeyWords { get; set; }
        public string? ParentCategory { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategorySlug { get; set; }
        public string? Categories { get; set; }
        public List<TgsForEachNews>? Tags { get; set; }
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
        [MaxLength(1000)]
        public string? CommentText { get; set; }
        public bool IsActive { get; set; }
        public StatusName? Status { get; set; }
        public int? CountSeeNews { get; set; }
        public string? IndexImageAddressAlt { get; set; }
        public string? IndexImageAddressTitle { get; set; }
        public List<CommentListDto>? Comments { get; set; }
        public string? UserName { get; set; }

        public string? Gif1Address { get; set; }
        public string? Gif2Address { get; set; }
        public string? Gif3Address { get; set; }
        public string? Gif4Address { get; set; }
        public string? Gif5Address { get; set; }
        public string? Gif6Address { get; set; }
        public string? Gif7Address { get; set; }
        public string? Gif8Address { get; set; }
        public string? Gif9Address { get; set; }
        public string? Gif10Address { get; set; }
        public string? Gif11Address { get; set; }
    }

    public class TgsForEachNews
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }

    
}
