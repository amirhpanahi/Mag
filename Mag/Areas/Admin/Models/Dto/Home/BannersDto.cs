using Microsoft.AspNetCore.Http;

namespace Mag.Areas.Admin.Models.Dto.Home
{
    public class BannersDto
    {
        public string? Banner1Address { get; set; }
        public string? Banner2Address { get; set; }
        public string? Banner3Address { get; set; }
        public string? Banner4Address { get; set; }
        public string? Banner5Address { get; set; }
        public string? Banner6Address { get; set; }
        public string? Banner7Address { get; set; }
        public IFormFile? Banner1 { get; set; }
        public IFormFile? Banner2 { get; set; }
        public IFormFile? Banner3 { get; set; }
        public IFormFile? Banner4 { get; set; }
        public IFormFile? Banner5 { get; set; }
        public IFormFile? Banner6 { get; set; }
        public IFormFile? Banner7 { get; set; }
    }
}
