namespace Mag.Areas.Admin.Models.Dto.Home
{
    public class IndexPageDto
    {
        public int NumberOfPublish { get; set; }
        public int NumberOfWateforConfirm { get; set; }
        public int NumberOfReject { get; set; }
        public int NumberOfDraft { get; set; }
        public int NumberOfDelete { get; set; }
        public int PercentOfWatit { get; set; }
    }
}
