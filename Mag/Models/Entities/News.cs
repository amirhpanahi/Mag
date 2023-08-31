namespace Mag.Models.Entities
{
    public class News
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
        public string? IndexImageAddressAlt { get; set; }
        public string? IndexImageAddressTitle { get; set; }
        public DateTime? RegisterNewsDate { get; set; }
        public string? RegisterNewsDatePersian { get; set; }
        public DateTime? PublishNewsDate { get; set; }
        public string? PublishNewsDatePersian { get; set; }
        public DateTime? DraftNewsDate { get; set; }
        public string? DraftNewsDatePersian { get; set; }
        public DateTime? DeleteNewsDate { get; set; }
        public string? DeleteNewsDatePersian { get; set; }
        public DateTime? RejecteNewsDate { get; set; }
        public string? RejecteNewsDatePersian { get; set; }
        public string WriterId { get; set; }
        public bool IsActive { get; set; }
        public StatusName? Status { get; set; }
        public string? NewsSummary { get; set; }

        public int? CountSeeNews { get; set; }

    }



    public enum StatusName
    {
        Publish,
        Draft,
        Delete,
        WaitingForConfirm,
        RejectedByAdmin
    }
}
