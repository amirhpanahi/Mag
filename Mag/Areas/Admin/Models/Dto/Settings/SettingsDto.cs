using Microsoft.AspNetCore.Http;

namespace Mag.Areas.Admin.Models.Dto.Settings
{
    public class SettingsDto
    {
        public int Id { get; set; }
        public string? SiteName { get; set; }
        public string? Title { get; set; }
        public string? LogoAddress { get; set; }
        public string? FavIconAddress { get; set; }

        public bool Instagram { get; set; }
        public bool Twitter { get; set; }
        public bool Facebook { get; set; }
        public bool Whatsapp { get; set; }
        public bool Telegram { get; set; }
        public bool Youtube { get; set; }
        public bool Linkedin { get; set; }
        public bool Gmail { get; set; }

        public string? InstagramLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? FacebookLink { get; set; }
        public string? WhatsappLink { get; set; }
        public string? TelegramLink { get; set; }
        public string? YoutubeLink { get; set; }
        public string? LinkedinLink { get; set; }
        public string? GmailLink { get; set; }

        public string? SeoDescription { get; set; }
        public string? KeyWords { get; set; }

        public string? ScriptHeader { get; set; }

        public string? TextForFooter { get; set; }
        public string? FooterMenu { get; set; }
        public string? Permissions { get; set; }
        public string? CopyrightText { get; set; }
        public string? ScriptFooter { get; set; }

        public IFormFile? LogoFile { get; set; }
        public IFormFile? FavIconFile { get; set; }
    }
}
