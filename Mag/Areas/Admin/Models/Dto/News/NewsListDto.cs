﻿using Mag.Models.Entities;

namespace Mag.Areas.Admin.Models.Dto.News
{
    public class NewsListDto
    {
        public string? NewsSummary { get; set; }
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
        public string? PublishTimePersain { get; set; }
        public string? WriterId { get; set; }
        public FullnameUser? WriterName { get; set; }

        public bool IsActive { get; set; }
        public bool IsSelectBychiefEditor { get; set; }
        public StatusName? Status { get; set; }
        public int? CountSeeNews { get; set; }
        public string? IndexImageAddressAlt { get; set; }
        public string? IndexImageAddressTitle { get; set; }
        public List<string>? CategoriesName { get; set; }
    }
    public class FullnameUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
