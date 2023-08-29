namespace Mag.Models.Dto.Settings
{
    public class SettingFotterDto
    {
        public string? TextForFooter { get; set; }
        public List<string>? FooterMenu { get; set; }
        public string? Permissions { get; set; }
        public string? CopyrightText { get; set; }
        public string? ScriptFooter { get; set; }

        public Dictionary<string,string>? SocialMedia { get; set; }

    }
}
