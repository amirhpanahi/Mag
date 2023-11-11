namespace Mag.Areas.Admin.Models.Dto.Home
{
    public class MainBannersDto
    {
        public string? Banner1Address { get; set; }
        public string? Banner1PhotoText { get; set; }
        public string? Banner1PhotoLink { get; set; }

        public string? Banner2Address { get; set; }
        public string? Banner2PhotoText { get; set; }
        public string? Banner2PhotoLink { get; set; }

        public string? Banner3Address { get; set; }
        public string? Banner3PhotoText { get; set; }
        public string? Banner3PhotoLink { get; set; }

        public string? Banner4Address { get; set; }
        public string? Banner4PhotoText { get; set; }
        public string? Banner4PhotoLink { get; set; }

        public string? Banner5Address { get; set; }
        public string? Banner5PhotoText { get; set; }
        public string? Banner5PhotoLink { get; set; }

        public string? Banner6Address { get; set; }
        public string? Banner6PhotoText { get; set; }
        public string? Banner6PhotoLink { get; set; }

        public IFormFile? Banner1 { get; set; }
        public IFormFile? Banner2 { get; set; }
        public IFormFile? Banner3 { get; set; }
        public IFormFile? Banner4 { get; set; }
        public IFormFile? Banner5 { get; set; }
        public IFormFile? Banner6 { get; set; }
    }
}
